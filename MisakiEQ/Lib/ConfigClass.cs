using System;
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
                    if(Configs.Connections[i].GetManageType()!="function")sw.WriteLine($"{Configs.Connections[i].GetName()}={Configs.Connections[i].ConfigWrite()}");
                }
                for (int i = 0; i < Configs.UserSetting.Count; i++)
                {
                    if (Configs.UserSetting[i].GetManageType() != "function")sw.WriteLine($"{Configs.UserSetting[i].GetName()}={Configs.UserSetting[i].ConfigWrite()}");
                }
                for (int i = 0; i < Configs.SNSSetting.Count; i++)
                {
                    if (Configs.SNSSetting[i].GetManageType() != "function") sw.WriteLine($"{Configs.SNSSetting[i].GetName()}={Configs.SNSSetting[i].ConfigWrite()}");
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
                Log.Logger.GetInstance().Debug("Config読込開始");
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

                            Log.Logger.GetInstance().Debug($"{line}");
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
            api.EEW.Config.Delay = (uint)(GetConfigValue("API_EEW_Delay") as long ? ?? long.MaxValue);
            api.EEW.Config.DelayDetectMode = (uint)(GetConfigValue("API_EEW_DelayDetectMode") as long? ?? long.MaxValue);
            api.EEW.Config.DelayDetectCoolDown = (uint)(GetConfigValue("API_EEW_DelayDetectCoolDown") as long? ?? long.MaxValue); 
            api.EQInfo.Config.Delay = (uint)(GetConfigValue("API_EQInfo_Delay") as long? ?? long.MaxValue);
            api.EQInfo.Config.Limit = (uint)(GetConfigValue("API_EQInfo_Limit") as long? ?? long.MaxValue);
            api.KyoshinAPI.Config.KyoshinDelayTime = (int)(GetConfigValue("API_K-moni_Delay") as long? ?? long.MaxValue);
            api.KyoshinAPI.Config.KyoshinFrequency = (int)(GetConfigValue("API_K-moni_Frequency") as long? ?? long.MaxValue);
            api.KyoshinAPI.Config.AutoAdjustKyoshinTime = (int)(GetConfigValue("API_K-moni_Adjust") as long? ?? long.MaxValue) * 60;
            var gui = APIs.GetInstance().KyoshinAPI.Config;
            gui.UserLong = (int)(GetConfigValue("USER_Pos_Long") as long? ?? long.MaxValue) / 10000.0;
            gui.UserLat = (int)(GetConfigValue("USER_Pos_Lat") as long? ?? long.MaxValue) / 10000.0;
            gui.UserDisplay = GetConfigValue("USER_Pos_Display") as bool? ?? false;
            var snd = Sound.Sounds.GetInstance().Config;
            snd.EEWVolume = (int)(GetConfigValue("Sound_Volume_EEW") as long? ?? 100);
            snd.EarthquakeVolume = (int)(GetConfigValue("Sound_Volume_Earthquake") as long? ?? 100);
            snd.TsunamiVolume = (int)(GetConfigValue("Sound_Volume_Tsunami") as long? ?? 100);
#if DEBUG||ADMIN
            Twitter.APIs.GetInstance().Config.TweetEnabled = (GetConfigValue("Twitter_Enable_Tweet") as bool? ?? false);
#endif
        }
        public IndexData? GetConfigClass(string name)
        {
            for(int i = 0; i<Configs.Connections.Count; i++)
            {
                if(Configs.Connections[i].GetName() == name)
                {
                    return Configs.Connections[i];
                }
            }
            for (int i = 0; i < Configs.UserSetting.Count; i++)
            {
                if (Configs.UserSetting[i].GetName() == name)
                {
                    return Configs.UserSetting[i];
                }
            }

            for (int i = 0; i < Configs.SNSSetting.Count; i++)
            {
                if (Configs.SNSSetting[i].GetName() == name)
                {
                    return Configs.SNSSetting[i];
                }
            }
            return null;
        }
        object GetConfigValue(string name)
        {
            var cls = GetConfigClass(name);
            if (cls == null) return 0;
            return (object)cls.GetValue();
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
            public readonly List<IndexData> SNSSetting = new();
            public readonly List<IndexData> SoundSetting = new();
            public Cfg()
            {
                Connections.Add(new IndexData("API_EEW_Delay","EEW標準遅延",unitName:"秒",displayMag:1000, description:"標準状態の緊急地震速報の遅延時間です。", min:1000, max:5000, def:1000));   //通常時の遅延(ms)
                Connections.Add(new IndexData("API_EEW_DelayDetectMode", "EEW検知遅延", unitName: "秒", displayMag: 1000, description: "検知状態の緊急地震速報の遅延時間です。", min:150, max:5000, def:200));     //検出時の遅延(ms)
                Connections.Add(new IndexData("API_EEW_DelayDetectCoolDown", "EEWモード移行時間", unitName: "秒", displayMag: 1000, description: "検知状態から標準状態に回復する時間です。", min:1000, max:10000, def:4000));     //検出から通常時に戻る時間(ms)
                Connections.Add(new IndexData("API_EQInfo_Delay", "地震情報遅延", unitName: "秒", displayMag: 1000, description: "地震情報全般の取得遅延時間です。\n地震と津波情報共通でリクエスト多過ぎるとエラー出ます。", min:2000, max:30000, def:3000));   //通常時の遅延(ms)
                Connections.Add(new IndexData("API_EQInfo_Limit", "地震情報取得項目数", description: "1回で地震情報を取得する数です。\n値を大きくすると1回あたりより多くの情報が更新されますが、その分遅くなります。", min:1, max:100, def:10));   //取得時の配列の数
                Connections.Add(new IndexData("API_K-moni_Delay", "強震モニタ遅延時間", description: "強震モニタの時刻からの遅延を設定できます。\n低い程低遅延ですが、更新されない可能性があります。", min:0, max:10000, def:1,unitName:"秒"));   //取得時の配列の数
                Connections.Add(new IndexData("API_K-moni_Frequency", "強震モニタ更新間隔", description: "強震モニタの更新間隔です。データ消費量を抑えたい時にお使いください。", min:1, max:5, def:1,unitName:"秒"));   //取得時の配列の数
                Connections.Add(new IndexData("API_K-moni_Adjust", "強震モニタ補正間隔", description: "強震モニタの時刻調整間隔です。自動で時刻補正する間隔を設定できます。", min:10, max:720, def:30,unitName:"分"));   //取得時の配列の数
                UserSetting.Add(new IndexData("USER_Pos_Lat", "所在地(緯度)", description: "ユーザーの緯度です。予測震度を表示させたい場合にお使いください。", min: 237000, max: 462000, def: 356896,displayMag:10000));   //取得時の配列の数
                UserSetting.Add(new IndexData("USER_Pos_Long", "所在地(経度)", description: "ユーザーの経度です。予測震度を表示させたい場合にお使いください。", min:1225000, max: 1460000, def: 1396983, displayMag:10000));   //取得時の配列の数
                UserSetting.Add(new IndexData("USER_Pos_Display", "強震モニタに座標表示", description: "ユーザーの経度経度情報を強震モニタに紫色で表示します。", def: false, "強震モニタに表示","強震モニタに非表示"));   //取得時の配列の数
#if ADMIN||DEBUG
                SNSSetting.Add(new IndexData("Twitter_Auth", "Twitter認証", "アカウント認証します","認証",WorkingTitle:"認証中..."));
                SNSSetting.Add(new IndexData("Twitter_Enable_Tweet", "自動ツイートの有効化", "自動でユーザーに地震情報をツイートします", def: false, "自動ツイートが有効", "自動ツイートが無効"));
#endif
                SoundSetting.Add(new IndexData("Sound_Volume_EEW", "EEWの通知音量", "緊急地震速報発生時に通知される音量を設定します。", def: 100, min: 0, max: 100, unitName: "%"));
                SoundSetting.Add(new IndexData("Sound_Volume_Earthquake", "地震情報の通知音量", "地震情報発表時に通知される音量を設定します。", def: 100, min: 0, max: 100, unitName: "%"));
                SoundSetting.Add(new IndexData("Sound_Volume_Tsunami", "津波情報の通知音量", "津波情報発表時に通知される音量を設定します。", def: 100, min: 0, max: 100, unitName: "%"));
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
            public IndexData(string name,string title, string description = "", bool def = false, string ToggleOnText = "", string ToggleOffText = "")
            {
                Name = name;
                Title = title;
                Description = description;
                BooleanDefault = def;
                BooleanValue = def;
                Type = "bool";
                BoolToggleOn = ToggleOnText;
                BoolToggleOff = ToggleOffText;
            }
            public IndexData(string name, string title, string description, string ReadyTitle, Action? action=null,string? WorkingTitle = null)
            {
                Description = description;
                Name = name;
                Title = title;
                Type = "function";
                FunctionAction = action;
                FunctionReady = ReadyTitle;
                FunctionWorking = WorkingTitle;

            }
            public void SetAction(Action act)
            {
                if (Type != "function") throw new InvalidOperationException($"{Type}型は関数設定できません！");
                FunctionAction = act;
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
                    "bool" => (object)(bool)BooleanValue,
                    _ => throw new NullReferenceException("タイプが一致しません。"),
                };
            }

            public object ConfigWrite()
            {
                return Type switch
                {
                    "long" => (object)LongValue,
                    "string" => StringValue.Replace("%","%%").Replace("=","%3D").Replace("\n","%0D"),
                    "bool" => BooleanValue,
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
            public void ExecuteAction()
            {
                if (Type != "function") throw new InvalidCastException($"{Type}型は実行できません。");
                Task.Run(() => { FunctionAction?.Invoke(); });
            }
            bool __buttonEnable = true;
            public bool ButtonEnable
            {
                get
                {
                    if (Type != "function") throw new InvalidCastException($"{Type}型は実行できません。");
                    return __buttonEnable;
                }
                set
                {
                    if(Type!="function") throw new InvalidCastException($"{Type}型は実行できません。");
                    if (__buttonEnable != value)
                    {
                        __buttonEnable = value;
                        try
                        {
                            ButtonChanged?.Invoke(this, EventArgs.Empty);
                        }catch(Exception ex)
                        {
                            Log.Logger.GetInstance().Warn(ex.Message);
                        }
                    }
                }
            }
            public EventHandler<EventArgs>? ButtonChanged = null;
            public string GetButton(bool IsWorking)
            {
                if (Type != "function") throw new InvalidCastException($"{Type}型は実行できません。");
                if (IsWorking) return FunctionWorking?? FunctionReady;
                else return FunctionReady;
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
                    _ => throw new ArgumentException("タイプが一致しません。"),
                };
            }
            public object GetMax()
            {
                return Type switch
                {
                    "long" => (object)LongMax,
                    _ => throw new ArgumentException("タイプが一致しません。"),
                };
            }
            public string GetToggleOffText()
            {
                return Type switch
                {
                    "bool" => BoolToggleOff,
                    _ => throw new ArgumentException("タイプが一致しません。")
                };
            }
            public string GetToggleOnText()
            {
                return Type switch
                {
                    "bool" => BoolToggleOn,
                    _ => throw new ArgumentException("タイプが一致しません。")
                };
            }

            public object GetDefault()
            {
                return Type switch
                {
                    "long" => (object)LongDefault,
                    "string"=> (object)StringDefault,
                    "bool"=>(object)BooleanDefault,
                    _ => throw new ArgumentException("タイプが一致しません。"),
                };
            }
            public string GetUnitName()
            {
                return Type switch
                {
                    "long" => UnitName,
                    _ => throw new ArgumentException("タイプが一致しません。"),
                };
            }
            public double GetDisplayMag()
            {
                return Type switch
                {
                    "long" => DisplayMag,
                    _ => throw new ArgumentException("タイプが一致しません。"),
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
            readonly string BoolToggleOn = "";
            readonly string BoolToggleOff = "";
            Action? FunctionAction = null;
            readonly string FunctionReady = "";
            readonly string? FunctionWorking = null;

        }
    }
}