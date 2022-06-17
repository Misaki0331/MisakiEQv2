﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MisakiEQ.Background;
using MisakiEQ.Log;
using static MisakiEQ.Lib.Config.Funcs;

namespace MisakiEQ.Lib.Config
{
    public class Funcs
    {
        static Funcs? singleton = null;
        public static Funcs GetInstance()
        {
            if (singleton == null)
            {
                singleton = new Funcs();
            }
            return singleton;
        }
        private Funcs()
        {

        }
        private const string CfgFile = "Config.cfg";

        public bool SaveConfig()
        {
            try
            {
                System.Reflection.FieldInfo[] fields = Configs.GetType().GetFields();
                using var sw = new StreamWriter(CfgFile, false, Encoding.UTF8);
                for(int i = 0; i < Configs.Connections.Count; i++)
                {
                    sw.WriteLine($"{Configs.Connections[i].GetName()}={Configs.Connections[i].ConfigWrite()}");
                }
                sw.Close();

                TmpConfigs = Configs.Clone();
                return true;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Error("ファイルの書込中にエラーが発生しました。");
                Logger.GetInstance().Error(ex);
                return false;
            }
        }
        public bool ReadConfig()
        {
            var sw = new Stopwatch();
            sw.Start();
            int PASS = 0, TOTAL = 0;
            try
            {
                using var sr = new StreamReader(CfgFile, Encoding.UTF8);
                while (true)
                {
                    var line = sr.ReadLine();
                    if (line == null) break;
                    var lines = line.Split('=');
                    if (lines.Length == 2)
                    {
                        try
                        {
                            SetConfigValue(lines[0],lines[1]);
                            PASS++;
                        }catch(Exception ex)
                        {
                            Logger.GetInstance().Error(ex);
                        }
                        TOTAL++;
                    }
                }
                sr.Close();
                sw.Stop();
                Logger.GetInstance().Debug($"コンフィグの読込に成功 計測時間:{(sw.ElapsedTicks / 10000.0):#,##0.0000}ms 読込数:{PASS}/{TOTAL}");
                return true;
            }
            catch (FileNotFoundException)
            {
                Logger.GetInstance().Warn("ファイルが見つかりませんでした。");
                return false;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Error($"コンフィグの読込に失敗 計測時間:{(sw.ElapsedTicks / 10000.0):#,##0.0000}ms");
                Logger.GetInstance().Error(ex);
                return false;
            }
        }
        public void ApplyConfig()
        {
            var api = APIs.GetInstance();
            api.EEW.Config.Delay = (uint)GetConfigValue("API_EEW_Delay");
            api.EEW.Config.DelayDetectMode = (uint)GetConfigValue("API_EEW_DelayDetectMode");
            api.EEW.Config.DelayDetectCoolDown = (uint)GetConfigValue("API_EEW_DelayDetectCoolDown"); 
            api.EQInfo.Config.Delay = (uint)GetConfigValue("API_EQInfo_Delay");
            api.EQInfo.Config.Limit = (uint)GetConfigValue("API_EQInfo_Limit");
            api.KyoshinAPI.Config.KyoshinDelayTime = (int)GetConfigValue("API_K-moni_Delay");
            api.KyoshinAPI.Config.KyoshinFrequency = (int)GetConfigValue("API_K-moni_Frequency");
            api.KyoshinAPI.Config.AutoAdjustKyoshinTime = (int)GetConfigValue("API_K-moni_Adjust")*60;
            var gui = APIs.GetInstance().KyoshinAPI.Config;
            gui.UserLong = (int)GetConfigValue("USER_Pos_Long");
            gui.UserLat = (int)GetConfigValue("USER_Pos_Lat");
        }
        IndexData? GetConfigClass(string name)
        {
            for(int i = 0; i<Configs.Connections.Count; i++)
            {
                if(Configs.Connections[i].GetName() == name)
                {
                    return Configs.Connections[i];
                }
            }
            return null;
        }
        long GetConfigValue(string name)
        {
            var cls = GetConfigClass(name);
            if (cls == null) return 0;
            return (long)cls.GetValue();
        }

        void SetConfigValue(string name, string value, bool IsThrow=true)
        {
            var cls = GetConfigClass(name);
            if (cls == null)
            {
                if (IsThrow) throw new ArgumentNullException($"\"{name}\"はConfig上に存在しません。");
                return;
            }
            cls.ConfigRead(value);
        }
        public void DiscardConfig()
        {
            Configs = TmpConfigs.Clone();
        }
        public Cfg Configs=new();
        public Cfg TmpConfigs=new();
        public class Cfg
        {
            public readonly List<IndexData> Connections=new();
            public readonly List<IndexData> UserSetting=new();
            public Cfg()
            {
                Connections.Add(new IndexData("API_EEW_Delay","EEW標準遅延",unitName:"秒",displayMag:1000, description:"標準状態の緊急地震速報の遅延時間です。", min:1000, max:5000, def:1000));   //通常時の遅延(ms)
                Connections.Add(new IndexData("API_EEW_DelayDetectMode", "EEW検知遅延", unitName: "秒", displayMag: 1000, description: "検知状態の緊急地震速報の遅延時間です。", min:150, max:5000, def:200));     //検出時の遅延(ms)
                Connections.Add(new IndexData("API_EEW_DelayDetectCoolDown", "EEWモード移行時間", unitName: "秒", displayMag: 1000, description: "検知状態から標準状態に回復する時間です。", min:1000, max:10000, def:4000));     //検出から通常時に戻る時間(ms)
                Connections.Add(new IndexData("API_EQInfo_Delay", "地震情報遅延", unitName: "秒", displayMag: 1000, description: "地震情報全般の取得遅延時間です。\n地震と津波情報共通でリクエスト多過ぎるとエラー出ます。", min:2000, max:30000, def:3000));   //通常時の遅延(ms)
                Connections.Add(new IndexData("API_EQInfo_Limit", "地震情報取得項目数", description: "1回で地震情報を取得する数です。\n値を大きくすると1回あたりより多くの情報が更新されますが、その分遅くなります。", min:1, max:100, def:10));   //取得時の配列の数
                Connections.Add(new IndexData("API_K-moni_Delay", "強震モニタ遅延時間", description: "強震モニタの時刻からの遅延を設定できます。\n低い程低遅延ですが、更新されない可能性があります。", min:0, max:5000, def:1,unitName:"秒"));   //取得時の配列の数
                Connections.Add(new IndexData("API_K-moni_Frequency", "強震モニタ更新間隔", description: "強震モニタの更新間隔です。データ消費量を抑えたい時にお使いください。", min:1, max:5, def:1,unitName:"秒"));   //取得時の配列の数
                Connections.Add(new IndexData("API_K-moni_Adjust", "強震モニタ補正間隔", description: "強震モニタの時刻調整間隔です。自動で時刻補正する間隔を設定できます。", min:10, max:720, def:30,unitName:"分"));   //取得時の配列の数
                UserSetting.Add(new IndexData("USER_Pos_Lat", "所在地(緯度)", description: "ユーザーの緯度です。予測震度を表示させたい場合にお使いください。", min: 237000, max: 462000, def: 356896,displayMag:10000));   //取得時の配列の数
                UserSetting.Add(new IndexData("USER_Pos_Long", "所在地(経度)", description: "ユーザーの経度です。予測震度を表示させたい場合にお使いください。", min:1225000, max: 1460000, def: 1396983, displayMag:10000));   //取得時の配列の数
            }
            public Cfg Clone()
            {
                return (Cfg)MemberwiseClone();
            }
        }
        public class IndexData
        {
            public IndexData(string name,string title, string description = "", long min = long.MinValue, long max = long.MaxValue, long def = 0, string unitName = "", double displayMag = 1)
            {
                Name = name;
                Title = title;
                Description = description;
                LongMin = min;
                LongMax = max;
                LongDefault = def;
                LongValue = def;
                UnitName = unitName;
                DisplayMag = displayMag;
                Type = "long";
            }

            public IndexData(string name, string title, string description = "",  string def = "")
            {
                Name = name;
                Title = title;
                Description = description;
                StringDefault = def;
                StringValue = def;
                Type = "string";
            }
            public IndexData(string name,string title,string description="",bool def = false)
            {
                Name = name;
                Title = title;
                Description = description;
                BooleanDefault = def;
                BooleanValue = def;
                Type = "bool";
            }
            public void SetValue(long val)
            {
                if (Type != "long") throw new ArgumentException($"[{Name}]は{Type}で管理されていますが、longで変更しようとしました。");
                if (val < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} < {LongMin}");
                if (val > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} > {LongMax}");
                LongValue = val;
            }
            public void SetValue(string val)
            {
                if (Type != "string") throw new ArgumentException($"[{Name}]は{Type}で管理されていますが、stringで変更しようとしました。");
                StringValue = val;
            }
            public void SetValue(bool val)
            {
                if(Type != "bool") throw new ArgumentException($"[{Name}]は{Type}で管理されていますが、boolで変更しようとしました。");
                BooleanValue = val;
            }
            public object GetValue()
            {
                return Type switch
                {
                    "long" => (object)(long)LongValue,
                    "string"=> (object)(string)StringValue,
                    "bool"=> (object)(bool)BooleanValue,
                    _ => throw new NullReferenceException("タイプが一致しません。"),
                };
            }

            public object ConfigWrite()
            {
                return Type switch
                {
                    "long" => (object)(long)LongValue,
                    "string" => (object)(string)StringValue.Replace("%","%%").Replace("=","%3D").Replace("\n","%0D"),
                    "bool" => (object)(string)(BooleanValue?"true":"false"),
                    _ => throw new NullReferenceException("タイプが一致しません。"),
                };
            }
            public void ConfigRead(string val)
            {
                switch (Type)
                {
                    case "long": 
                        long value= long.Parse(val);
                        if (value < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} < {LongMin}");
                        if (value > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} > {LongMax}");
                        LongValue = value;
                        break;
                    case "string": 
                        string? str = (string?)val;
                        if (string.IsNullOrEmpty(str)) str = string.Empty;
                        StringValue = str.Replace("%0D", "\n").Replace("%3D", "=").Replace("%%", "%");
                        break;
                    case "bool":
                        if (val.ToLower() == "true"|| val == "0")
                        {
                            BooleanValue = true;
                        }else if (val.ToLower() == "false"||val=="1")
                        {
                            BooleanValue = false;
                        }
                        else
                        {
                            throw new ArgumentException($"bool型ではない、もしくは正しく検出できませんでした。val=\"{val}\"", nameof(val));
                        }
                        break;
                }
            }
            public string GetName()
            {
                return Name;
            }
            public string GetManageType()
            {
                return Type;
            }
            public string GetTitle()
            {
                return Title;
            }
            public string GetDescription()
            {
                return Description;
            }
            public object GetMin()
            {
                return Type switch
                {
                    "long" => (object)LongMin,
                    _ => throw new NullReferenceException("タイプが一致しません。"),
                };
            }
            public object GetMax()
            {
                return Type switch
                {
                    "long" => (object)LongMax,
                    _ => throw new NullReferenceException("タイプが一致しません。"),
                };
            }

            public object GetDefault()
            {
                return Type switch
                {
                    "long" => (object)LongDefault,
                    "string"=> (object)StringDefault,
                    "bool"=>(object)BooleanDefault,
                    _ => throw new NullReferenceException("タイプが一致しません。"),
                };
            }
            public string GetUnitName()
            {
                return Type switch
                {
                    "long" => UnitName,
                    _ => throw new NullReferenceException("タイプが一致しません。"),
                };
            }
            public double GetDisplayMag()
            {
                return Type switch
                {
                    "long" => DisplayMag,
                    _ => throw new NullReferenceException("タイプが一致しません。"),
                };
            }

            
            readonly string Type;
            readonly string Name;
            readonly string Title;
            readonly string UnitName = "";
            readonly string Description;
            long LongValue;
            readonly long LongMin;
            readonly long LongMax;
            readonly long LongDefault;
            readonly double DisplayMag;
            string StringValue="";
            readonly string StringDefault="";
            bool BooleanValue = false;
            readonly bool BooleanDefault = false;

        }
    }
}