using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MisakiEQ.Background;
using static MisakiEQ.Lib.Config.Funcs;

namespace MisakiEQ.Lib.Config
{
    public class Funcs
    {
        static Funcs? singleton = null;
        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        /// <returns>Configのインスタンス</returns>
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
        /// <summary>
        /// 現在のコンフィグ情報を保存します。
        /// </summary>
        /// <returns>保存が成功したかどうかのbool値</returns>
        public bool SaveConfig()
        {
            try
            {
                var sr = new Stopwatch();
                sr.Start();
                Log.Instance.Info("Config書込開始");
                System.Reflection.FieldInfo[] fields = Configs.GetType().GetFields();
                using var sw = new StreamWriter(CfgFile, false, Encoding.UTF8);
                for(int i = 0; i < Configs.Data.Count; i++)
                {
                    var config=Configs.Data[i].Setting;
                    for (int j = 0; j < config.Count; j++) sw.Write($"{config[j].Config}");
                }
                sw.Close();
                OverrideTemplates();
                sr.Stop();
                Log.Instance.Debug($"Config書込完了 計測時間:{(sr.ElapsedTicks / 10000.0):#,##0.0000}ms");
                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.Error("ファイルの書込中にエラーが発生しました。");
                Log.Instance.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// ファイルからコンフィグ情報を読み込みます。
        /// </summary>
        /// <returns>読込が成功したかどうかのbool値</returns>
        public bool ReadConfig()
        {
            var sw = new Stopwatch();
            sw.Start();
            int PASS = 0, TOTAL = 0;
            try
            {
                Log.Instance.Debug("Config読込開始");
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

                            Log.Instance.Debug($"{line}");
                            PASS++;
                        }catch(Exception ex)
                        {
                            Log.Instance.Error(ex);
                        }
                        TOTAL++;
                    }
                }
                sr.Close();
                OverrideTemplates();
                sw.Stop();
                Log.Instance.Debug($"コンフィグの読込に成功 計測時間:{(sw.ElapsedTicks / 10000.0):#,##0.0000}ms 読込数:{PASS}/{TOTAL}");
                return true;
            }
            catch (FileNotFoundException)
            {
                Log.Instance.Warn("ファイルが見つかりませんでした。");
                return false;
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"コンフィグの読込に失敗 計測時間:{(sw.ElapsedTicks / 10000.0):#,##0.0000}ms");
                Log.Instance.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// コンフィグをそれぞれの機能に変数の値を上書きします。
        /// </summary>
        public void ApplyConfig()
        {
            var api = APIs.GetInstance();
            api.EEW.Config.Delay = (uint)(GetConfigValue("API_EEW_Delay") as long ? ?? long.MaxValue);
            api.EEW.Config.DelayDetectMode = (uint)(GetConfigValue("API_EEW_DelayDetectMode") as long? ?? long.MaxValue);
            api.EEW.Config.DelayDetectCoolDown = (uint)(GetConfigValue("API_EEW_DelayDetectCoolDown") as long? ?? long.MaxValue); 
            api.EEW.Config.IsWarningOnlyInDMDATA = (GetConfigValue("API_EEW_DMDATA_OnlyWarn") as bool? ?? false); 
            api.EQInfo.Config.Delay = (uint)(GetConfigValue("API_EQInfo_Delay") as long? ?? long.MaxValue);
            api.EQInfo.Config.Limit = (uint)(GetConfigValue("API_EQInfo_Limit") as long? ?? long.MaxValue);
            api.KyoshinAPI.Config.KyoshinDelayTime = (int)(GetConfigValue("API_K-moni_Delay") as long? ?? long.MaxValue);
            api.KyoshinAPI.Config.KyoshinFrequency = (int)(GetConfigValue("API_K-moni_Frequency") as long? ?? long.MaxValue);
            api.KyoshinAPI.Config.AutoAdjustKyoshinTime = (int)(GetConfigValue("API_K-moni_Adjust") as long? ?? long.MaxValue) * 60;
            api.Jalert.Config.Delay = (uint)(GetConfigValue("API_J-ALERT_Delay") as long? ?? long.MaxValue) * 1000;
            api.Jalert.Config.IsDisplay = (bool)(GetConfigValue("GUI_Popup_J-ALERT") as bool? ?? true);
            var gui = APIs.GetInstance().KyoshinAPI.Config;
            gui.UserLong = (int)(GetConfigValue("USER_Pos_Long") as long? ?? long.MaxValue) / 10000.0;
            gui.UserLat = (int)(GetConfigValue("USER_Pos_Lat") as long? ?? long.MaxValue) / 10000.0;
            gui.UserDisplay = GetConfigValue("USER_Pos_Display") as bool? ?? false;
            var snd = Sound.Sounds.GetInstance().Config;
            snd.EEWVolume = (int)(GetConfigValue("Sound_Volume_EEW") as long? ?? 100);
            snd.EarthquakeVolume = (int)(GetConfigValue("Sound_Volume_Earthquake") as long? ?? 100);
            snd.TsunamiVolume = (int)(GetConfigValue("Sound_Volume_Tsunami") as long? ?? 100);
            snd.IsMute = GetConfigValue("Sound_All_Mute") as bool? ?? false;
            var tray = GUI.TrayHub.GetInstance()?.ConfigData;
            if (tray != null)
            {
                tray.IsWakeSimpleEEW = GetConfigValue("GUI_Popup_EEW_Compact") as bool? ?? true;
                tray.IsTopSimpleEEW = GetConfigValue("GUI_TopMost_EEW_Compact") as bool? ?? true;
                tray.NoticeNationWide = Struct.ConfigBox.Notification_EEW_Nationwide.GetIndex(GetConfigValue("Notification_EEW_Nationwide") as long? ?? 9);
                tray.NoticeArea = Struct.ConfigBox.Notification_EEW_Area.GetIndex(GetConfigValue("Notification_EEW_Area") as long? ?? 8);
            }
            Lib.ToastNotification.IsNewNotification= (bool)(GetConfigValue("Notification_Popup_Notify") as bool? ?? false);
#if DEBUG||ADMIN
            Twitter.APIs.GetInstance().Config.TweetEnabled = (GetConfigValue("Twitter_Enable_Tweet") as bool? ?? true);
            Twitter.APIs.GetInstance().Config.IsTweetJ_ALERT = (GetConfigValue("Twitter_J-ALERT_Tweet") as bool? ?? true);

            Lib.Misskey.APIData.Config.IsEnableEarthquakeNote = (GetConfigValue("Misskey_Enable_Note") as bool? ?? true);
            Lib.Misskey.APIData.Config.IsEnableJAlertNote = (GetConfigValue("Misskey_J-ALERT_Note") as bool? ?? true);
            MisakiEQ.Funcs.Misskey.GetInstance().EEWDelayTime = (int)(GetConfigValue("Misskey_EEW_Delay") as long? ?? 2000);
#endif
        }
        /// <summary>
        /// 設定中のコンフィグから元に戻すときのクローンを作成する
        /// </summary>
        private void OverrideTemplates()
        {
            ConfigTemplates.Clear();
            for (int i = 0; i < Configs.Data.Count; i++)
            {
                var config = Configs.Data[i].Setting;
                for (int j = 0; j < config.Count; j++)
                {
                    if (config[j].Type == "combo")
                    {
                        ConfigTemplates.Add(new(config[j].Name, $"={config[j].Value}"));
                    }
                    else
                    if (config[j].ValueDefaultable()) ConfigTemplates.Add(new(config[j].Name, $"{config[j].Value}"));
                }
            }
            for (int i = 0; i < ConfigTemplates.Count; i++) SetConfigValue(ConfigTemplates[i].Key, ConfigTemplates[i].Value, false);
        }
        /// <summary>
        /// 名前からコンフィグのクラスを取得します。<br/>
        /// 一致する名前がなければnullが返ります。
        /// </summary>
        /// <param name="name">登録されている名前</param>
        /// <returns>一致するコンフィグクラス<br/>
        /// 存在しない場合はnullが返ります</returns>
        public IndexData? GetConfigClass(string name)
        {
                for (int i = 0; i < Configs.Data.Count; i++)
                {
                    var config = Configs.Data[i].Setting;
                    for (int j = 0; j < config.Count; j++)
                    {
                        if (config[j].Name == name) return config[j];
                    }
                }
                return null;
                
            
        }
        /// <summary>
        /// nameからコンフィグの値を取得します。
        /// </summary>
        /// <param name="name">取得するコンフィグの名前</param>
        /// <returns>名前に対応する値<br/>無ければnullが返ります。</returns>
        object? GetConfigValue(string name)
        {
            var cls = GetConfigClass(name);
            if (cls == null) return null;
            return cls.Value;
        }

        /// <summary>
        /// 対応する名前のコンフィグに値を書き込みます。
        /// </summary>
        /// <param name="name">値を設定したい名前</param>
        /// <param name="value">設定する値</param>
        /// <param name="IsThrow">存在しない時に例外をスローするか</param>
        /// <exception cref="ArgumentNullException"></exception>
        void SetConfigValue(string name, string value, bool IsThrow=true)
        {
            var cls = GetConfigClass(name);
            if (cls == null)
            {
                if (IsThrow) throw new ArgumentException($"\"{name}\"はConfig上に存在しません。");
                return;
            }
            cls.Config = value;
        }
        private class ConfigTemplate
        {
            public ConfigTemplate(string key,string value)
            {
                Key = key;
                Value = value;
            }
            public string Key = string.Empty;
            public string Value=string.Empty;
        }
        readonly List<ConfigTemplate> ConfigTemplates=new();
        public void DiscardConfig()
        {
            Log.Instance.Debug($"現在の設定が保存せずに破棄された為、コンフィグを前の状態に戻します。");
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < ConfigTemplates.Count; i++) SetConfigValue(ConfigTemplates[i].Key, ConfigTemplates[i].Value, false);
            sw.Stop();
            Log.Instance.Debug($"コンフィグを前の状態に戻しました。処理時間 : {sw.Elapsed}");
        }
        public Cfg Configs=new();
        public class Cfg
        {
            public readonly List<ConfigGroup> Data = new();
            public Cfg()
            {
                GetGroup("通信設定",true)?.Add(new IndexData("API_EEW_Delay","EEW標準遅延",unitName:"秒",displayMag:1000, description:"標準状態の緊急地震速報の遅延時間です。", min:1000, max:5000, def:1000));   //通常時の遅延(ms)
                GetGroup("通信設定",true)?.Add(new IndexData("API_EEW_DelayDetectMode", "EEW検知遅延", unitName: "秒", displayMag: 1000, description: "検知状態の緊急地震速報の遅延時間です。", min:150, max:5000, def:200));     //検出時の遅延(ms)
                GetGroup("通信設定",true)?.Add(new IndexData("API_EEW_DelayDetectCoolDown", "EEWモード移行時間", unitName: "秒", displayMag: 1000, description: "検知状態から標準状態に回復する時間です。", min:1000, max:10000, def:4000));     //検出から通常時に戻る時間(ms)
                GetGroup("通信設定",true)?.Add(new IndexData("API_EEW_DMDATA_OnlyWarn", "DMDATA取得モード", "警報のみ受け取るか、予報も受け取るかを選択できます。", def: false, "警報のみ取得","予報・警報を取得"));   
                GetGroup("通信設定",true)?.Add(new IndexData("API_EQInfo_Delay", "地震情報遅延", unitName: "秒", displayMag: 1000, description: "地震情報全般の取得遅延時間です。\n地震と津波情報共通でリクエスト多過ぎるとエラー出ます。", min:2000, max:30000, def:3000));   //通常時の遅延(ms)
                GetGroup("通信設定",true)?.Add(new IndexData("API_EQInfo_Limit", "地震情報取得項目数", description: "1回で地震情報を取得する数です。\n値を大きくすると1回あたりより多くの情報が更新されますが、その分遅くなります。", min:1, max:100, def:10));   //取得時の配列の数
                GetGroup("通信設定",true)?.Add(new IndexData("API_K-moni_Delay", "強震モニタ遅延時間", description: "強震モニタの時刻からの遅延を設定できます。\n低い程低遅延ですが、更新されない可能性があります。", min:0, max:5, def:1,unitName:"秒"));   //取得時の配列の数
                GetGroup("通信設定",true)?.Add(new IndexData("API_K-moni_Frequency", "強震モニタ更新間隔", description: "強震モニタの更新間隔です。データ消費量を抑えたい時にお使いください。", min:1, max:5, def:1,unitName:"秒"));   //取得時の配列の数
                GetGroup("通信設定",true)?.Add(new IndexData("API_K-moni_Adjust", "強震モニタ補正間隔", description: "強震モニタの時刻調整間隔です。自動で時刻補正する間隔を設定できます。", min:10, max:720, def:30,unitName:"分"));   //取得時の配列の数
                GetGroup("通信設定",true)?.Add(new IndexData("API_J-ALERT_Delay", "Jアラート遅延時間", description: "Jアラートの更新間隔です。", min:3, max:60, def:30,unitName:"秒"));   
                GetGroup("ユーザー設定",true)?.Add(new IndexData("USER_Pos_Lat", "所在地(緯度)", description: "ユーザーの緯度です。予測震度を表示させたい場合にお使いください。", min: 237000, max: 462000, def: 356896,displayMag:10000));   //取得時の配列の数
                GetGroup("ユーザー設定",true)?.Add(new IndexData("USER_Pos_Long", "所在地(経度)", description: "ユーザーの経度です。予測震度を表示させたい場合にお使いください。", min:1225000, max: 1460000, def: 1396983, displayMag:10000));   //取得時の配列の数
                GetGroup("ユーザー設定", true)?.Add(new IndexData("USER_Pos_Result", "該当地域名", "緯度・経度に対応されるであろう該当地域名です。"));
                GetGroup("ユーザー設定",true)?.Add(new IndexData("USER_Pos_Display", "強震モニタに座標表示", description: "ユーザーの経度経度情報を強震モニタに紫色で表示します。", def: false, "強震モニタに表示","強震モニタに非表示"));   //取得時の配列の数
                GetGroup("通信設定",true)?.Add(new IndexData("Kyoshin_Time_Adjust", "強震モニタ時刻調整", description: "強震モニタの時刻調整を実行します。", "時刻調整実行", WorkingTitle: "時刻調整中...", action:new Action(async() => { await APIs.GetInstance().KyoshinAPI.FixKyoshinTime(); })));//取得時の配列の数
                GetGroup("通信設定", true)?.Add(new IndexData("Kyoshin_Time", "強震モニタ時刻", "強震モニタ上の最終更新時刻です。"));
                GetGroup("Project DM-Data Service", true)?.Add(new IndexData("DMDATA_AuthFunction", "アプリ連携", "MisakiEQでDMDATAの緊急地震速報データを取得できるようにします。", "認証", WorkingTitle: "ブラウザで認証中..."));
#if ADMIN||DEBUG
                GetGroup("SNS設定",true)?.Add(new IndexData("Twitter_Auth", "Twitter認証", "アカウント認証します","認証",WorkingTitle:"認証中..."));
                GetGroup("SNS設定", true)?.Add(new IndexData("Twitter_Auth_Info", "Twitter認証情報", ""));
                GetGroup("SNS設定", true)?.Add(new IndexData("Twitter_Auth_UserID", "ユーザーID", ""));
                GetGroup("SNS設定", true)?.Add(new IndexData("Twitter_Auth_UserName", "ユーザー名", ""));
                GetGroup("SNS設定", true)?.Add(new IndexData("Twitter_Auth_Tweet", "ツイート数", ""));
                GetGroup("SNS設定", true)?.Add(new IndexData("Twitter_Auth_Follower", "フォロワー数", ""));
                GetGroup("SNS設定", true)?.Add(new IndexData("Twitter_Enable_Tweet", "自動ツイートの有効化", "自動でユーザーに地震情報をツイートします", def: true, "Twitterへの自動ツイートが有効", "Twitterへの自動ツイートが無効"));
                GetGroup("SNS設定", true)?.Add(new IndexData("Twitter_J-ALERT_Tweet", "J-ALERT配信有効化", "自動でユーザーにJアラートをツイートします", def: false, "Twitterへの自動ツイートが有効", "Twitterへの自動ツイートが無効"));
                GetGroup("SNS設定", true)?.Add(new IndexData("Misskey_Enable_Note", "自動ノートの有効化", "自動でユーザーに地震情報をMisskeyにノートします。", def: false, "Misskeyへの自動ノートが有効", "Misskeyへの自動ノートが無効"));
                GetGroup("SNS設定", true)?.Add(new IndexData("Misskey_J-ALERT_Note", "J-ALERT配信有効化", "自動でユーザーに地震情報をMisskeyにノートします。", def: false, "Misskeyへの自動ノートが有効", "Misskeyへの自動ノートが無効"));
                GetGroup("SNS設定", true)?.Add(new IndexData("Misskey_EEW_Delay", "EEWノート遅延", unitName: "秒", displayMag: 1000, description: "遅延時間です。", min: 0, max: 10000, def: 2000));   //通常時の遅延(ms)

#endif
                GetGroup("サウンド設定",true)?.Add(new IndexData("Sound_Volume_EEW", "EEWの通知音量", "緊急地震速報発生時に通知される音量を設定します。", def: 100, min: 0, max: 100, unitName: "%"));
                GetGroup("サウンド設定",true)?.Add(new IndexData("Sound_Volume_Earthquake", "地震情報の通知音量", "地震情報発表時に通知される音量を設定します。", def: 100, min: 0, max: 100, unitName: "%"));
                GetGroup("サウンド設定",true)?.Add(new IndexData("Sound_Volume_Tsunami", "津波情報の通知音量", "津波情報発表時に通知される音量を設定します。", def: 100, min: 0, max: 100, unitName: "%"));
                GetGroup("サウンド設定",true)?.Add(new IndexData("Sound_All_Mute", "完全ミュート", "音声デバイスがない時などはこのチェックボックスを有効にすることで軽量化されます。", def: false, "有効(再生されません)","無効"));

                GetGroup("緊急地震速報発表時", true)?.Add(new IndexData("GUI_Popup_EEW_Compact", "簡易情報のポップ表示", "緊急地震速報が発令されると簡易ウィンドウがポップアップされます。", true, "ポップアップ表示", "ポップアップ非表示"));
                GetGroup("緊急地震速報発表時", true)?.Add(new IndexData("GUI_TopMost_EEW_Compact", "簡易情報の表示モード", "簡易ウィンドウが前面表示されます。", true, "前面表示", "標準表示"));
                GetGroup("J-ALERT", true)?.Add(new IndexData("GUI_Popup_J-ALERT", "全画面ポップアップ", "Jアラート発令時に自動的に全画面でポップアップ表示されます。", true, "表示する", "表示しない"));
                GetGroup("アプリ情報", true)?.Add(new IndexData("AppInfo_Uptime", "アプリ起動時間", "起動してからの経過時間です。"));
                GetGroup("アプリ情報", true)?.Add(new IndexData("AppInfo_UsingAPI", "使用中のEEW API", "EEW APIの使用状況"));
                GetGroup("通知設定", true)?.Add(new IndexData("Notification_EEW_Nationwide", "EEW全国通知条件", "全国共通で緊急地震速報を通知する条件を設定します", 9,new string[] { "7", "≧6+", "≧6-", "≧5+", "≧5-", "≧4", "≧3", "≧2", "≧1", "ALL","Warning only","None"}));
                GetGroup("通知設定", true)?.Add(new IndexData("Notification_EEW_Area", "EEW地域通知条件", "お住まいの地域で緊急地震速報を通知する条件を設定します", 8,new string[] { "7", "≧6+", "≧6-", "≧5+", "≧5-", "≧4", "≧3", "≧2", "≧1", "≧0", "None"}));
                GetGroup("通知設定", true)?.Add(new IndexData("Notification_Popup_Notify", "トースト通知タイプ", "通知タイプを設定します。", def:false, "Windows プッシュ通知", "Windows Form標準通知"));
                GetGroup("Debug Mode", true)?.Add(new IndexData("Debug_Input", "Misskey", "デバック用",""));
                GetGroup("Debug Mode", true)?.Add(new IndexData("Debug_Function", "ノート投稿", "デバック用", "Execute", WorkingTitle: "Working..."));

            }

            /// <summary>
            /// 値リストをコピーします。
            /// </summary>
            /// <returns>クローンされたコンフィグクラス</returns>
            public Cfg Clone()
            {
                return (Cfg)MemberwiseClone();
            }
            /// <summary>
            /// グループを取得します。
            /// 存在しない場合はnullを返すか、新しいグループを返します。
            /// </summary>
            /// <param name="name">グループ名</param>
            /// <param name="Create">存在しない時にグループを作成するか</param>
            /// <returns>対応するグループ</returns>
            public List<IndexData>? GetGroup(string name, bool Create = false)
            {
                for(int i = 0; i < Data.Count; i++)
                {
                    if (Data[i].Name==name)return Data[i].Setting;
                }
                if (Create)
                {
                    Data.Add(new(name));
                    return Data[^1].Setting;
                }
                return null;
            }
            public class ConfigGroup
            {
                public ConfigGroup(string name){
                    _name = name;
                }
                
                readonly string _name="";
                public string Name { get => _name; }
                public readonly List<IndexData> Setting=new();
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
            public IndexData(string name, string title, string description)
            {
                Description = description;
                Name = name;
                Title = title;
                Type = "readonly";
            }
            public IndexData(string name, string title, string description, int def, List<string> list)
            {
                Type = "combo";
                Description = description;
                Name = name;
                LongDefault = def;
                LongValue = def;
                Title = title;
                for (int i = 0; i < list.Count; i++) ComboStrings.Add(list[i]);

            }
            public IndexData(string name, string title, string description, int def, string[] list)
            {
                Type = "combo";
                Description = description;
                Name = name;
                LongDefault = def;
                LongValue = def;
                Title = title;
                for (int i = 0; i < list.Length; i++) ComboStrings.Add(list[i]);

            }
            /// <summary>
            /// ラムダ式を設定します。
            /// IndexDataのタイプが Function である必要があります。
            /// そうでない場合は例外がスローされます。
            /// </summary>
            /// <param name="act">実行関数</param>
            /// <exception cref="InvalidOperationException"></exception>
            public void SetAction(Action act)
            {
                //if (Type != "function") throw new InvalidOperationException($"{Type}型は関数設定できません！");
                FunctionAction = act;
            }
            public EventHandler? ValueChanged = null;

            public bool ValueDefaultable()
            {
                return Type switch
                {
                    "long" => true,
                    "string" => true,
                    "bool" => true,
                    "combo" => true,
                    _ => false
                };
            }
            /// <summary>
            /// 値を設定または取得します。
            /// </summary>
            public object Value
            {
                get
                {
                    return Type switch
                    {
                        "long" => LongValue,
                        "string" => StringValue,
                        "bool" => BooleanValue,
                        "readonly" => StringValue,
                        "combo" => LongValue,
                        _ => throw new InvalidDataException($"[{Name}]は値がありません。"),
                    };
                }
                set
                {
                    switch (Type)
                    {
                        case "long":
                            if (value.GetType() == typeof(short))
                            {
                                if (LongMin < short.MinValue || LongMax > short.MaxValue) throw new ArgumentOutOfRangeException($"[{Name}]の設定可能値はshort型からオーバーフローしています。long型で変更してください。");
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} < {LongMin}");
                                if ((long)value > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} > {LongMax}");
                                if (LongValue != (long)value)
                                {
                                    LongValue = (long)value;
                                    FunctionAction?.Invoke();
                                }
                            }
                            else
                            if (value.GetType() == typeof(int))
                            {
                                if (LongMin < int.MinValue || LongMax > int.MaxValue) throw new ArgumentOutOfRangeException($"[{Name}]の設定可能値はint型オーバーフローしています。long型で変更してください。");
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} < {LongMin}");
                                if ((long)value > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} > {LongMax}");
                                if (LongValue != (long)value)
                                {
                                    LongValue = (long)value;
                                    FunctionAction?.Invoke();
                                }
                            }
                            else if (value.GetType() == typeof(long))
                            {
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} < {LongMin}");
                                if ((long)value > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} > {LongMax}");
                                if (LongValue != (long)value)
                                {
                                    LongValue = (long)value;
                                    FunctionAction?.Invoke();
                                }
                            }
                            else
                                throw new InvalidCastException($"[{Name}]は整数型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            break;
                        case "string":
                            if (value.GetType() != typeof(string)) throw new InvalidCastException($"[{Name}]はstring型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            if ((string)value!=StringValue)
                            {
                                StringValue = (string)value;
                                try { ValueChanged?.Invoke(this, EventArgs.Empty); } catch(Exception ex) { Log.Instance.Warn(ex.Message); }
                            }
                            break;
                        case "bool":
                            if(value.GetType()!=typeof(bool)) throw new InvalidCastException($"[{Name}]はbool型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            BooleanValue = (bool)value;
                            break;
                        case "readonly":
                            if (value.GetType() != typeof(string)) throw new InvalidCastException($"[{Name}]はstring型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            if ((string)value != StringValue)
                            {
                                StringValue = (string)value;
                                try { ValueChanged?.Invoke(this, EventArgs.Empty); } catch (Exception ex) { Log.Instance.Warn(ex.Message); }
                            }
                            break;
                        case "combo":
                            if (value.GetType() == typeof(string))
                            {
                                for(int i = 0; i < ComboStrings.Count; i++)
                                {
                                    if(ComboStrings[i] == (string)value)
                                    {
                                        LongValue = i;
                                        break;
                                    }
                                    if(i == ComboStrings.Count - 1)
                                        throw new ArgumentException($"\"{value}\"は[{Name}]のコレクションの中には存在しませんでした。", nameof(value));
                                }
                            }
                            else if(value.GetType() == typeof(int))
                            {
                                int v = (int)value;
                                if (v < 0 || v >= ComboStrings.Count) throw new ArgumentOutOfRangeException($"[{Name}]のコレクションに対応する配列が見つかりません。設定した値:{value} 配列数:{ComboStrings.Count}");
                                LongValue = v;
                            }
                            else if (value.GetType() == typeof(long))
                            {
                                long v = (long)value;
                                if (v < 0 || v >= ComboStrings.Count) throw new ArgumentOutOfRangeException($"[{Name}]のコレクションに対応する配列が見つかりません。設定した値:{value} 配列数:{ComboStrings.Count}");
                                LongValue = v;
                            }
                            else
                            {
                                throw new InvalidCastException($"[{Name}]はstring型,int型,long型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            }
                            break;
                        default:
                            throw new InvalidDataException($"[{Name}は値を設定することができません。");
                    }
                }
            }
            /// <summary>
            /// コンフィグファイルの読書き用に使用されます。
            /// </summary>
            public string Config
            {
                get
                {
                    if (Type == "function"||Type== "readonly") return string.Empty;
                    return Name+"="+Type switch
                    {
                        "long" => LongValue.ToString(),
                        "string" => StringValue.Replace("%", "%%").Replace("=", "%3D").Replace("\n", "%0D"),
                        "bool" => BooleanValue.ToString(),
                        "combo" => ComboStrings[(int)LongValue].Replace("%", "%%").Replace("=", "%3D").Replace("\n", "%0D"),
                        _ => throw new NullReferenceException("タイプが一致しません。"),
                    }+Environment.NewLine;
                }
                set
                {
                    switch (Type)
                    {
                        case "long":
                            long val = long.Parse(value);
                            if (val < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} < {LongMin}");
                            if (val > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} > {LongMax}");
                            LongValue = val;
                            break;
                        case "string":
                            string? str = (string?)value;
                            if (string.IsNullOrEmpty(str)) str = string.Empty;
                            StringValue = str.Replace("%0D", "\n").Replace("%3D", "=").Replace("%%", "%");
                            break;
                        case "bool":
                            if (value.ToLower() == "true" || value == "0")
                            {
                                BooleanValue = true;
                            }
                            else if (value.ToLower() == "false" || value == "1")
                            {
                                BooleanValue = false;
                            }
                            else
                            {
                                throw new ArgumentException($"bool型ではない、もしくは正しく検出できませんでした。val=\"{value}\"", nameof(value));
                            }
                            break;
                        case "combo":
                            if (value.StartsWith("="))
                            {
                                if(int.TryParse(value.Replace("=",""),out int num))
                                {
                                    if (ComboStrings.Count > num && 0 <= num)
                                    {
                                        LongValue = num;
                                        break;
                                    }
                                    else throw new ArgumentOutOfRangeException(nameof(value), $"\"{num}\" は \"{ComboStrings.Count}\" の範囲外です。");
                                }
                                else
                                {
                                    throw new InvalidDataException($"\"{value}\"は数値に変換できませんでした。");
                                }
                            }
                            for (int i = 0; i < ComboStrings.Count; i++)
                            {
                                string vals = value.Replace("%0D", "\n").Replace("%3D", "=").Replace("%%", "%");
                                if (ComboStrings[i] == vals)
                                {
                                    LongValue = i;
                                    break;
                                }

                                if (i == ComboStrings.Count - 1)
                                    throw new ArgumentException($"\"{vals}\"は[{Name}]のコレクションの中には存在しませんでした。", nameof(value));
                            }
                            break;
                    }
                }
            }
            /// <summary>
            /// Function関数の場合は関数が実行されます。<br/>
            /// そうでない場合は例外がスローされます。
            /// </summary>
            /// <exception cref="InvalidCastException"></exception>
            public void ExecuteAction()
            {
                if (Type != "function") throw new InvalidCastException($"{Type}型は実行できません。");
                Task.Run(() => { FunctionAction?.Invoke(); });
            }
            bool __buttonEnable = true;
            /// <summary>
            /// ボタンが使用できるかどうかを設定します。<br/>
            /// Function 型では無い場合は例外がスローされます。
            /// </summary>
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
                            Log.Instance.Warn(ex.Message);
                        }
                    }
                }
            }
            /// <summary>
            /// ボタンの有効状態が変更された際に実行されるイベントです。
            /// </summary>
            public EventHandler<EventArgs>? ButtonChanged = null;
            /// <summary>
            /// 有効中かそれ以外でボタンのテキストに設定する文字列を取得します。
            /// Function型以外は例外がスローされます。
            /// </summary>
            /// <param name="IsWorking">有効中かどうか</param>
            /// <returns>対応する文字列型</returns>
            /// <exception cref="InvalidCastException"></exception>
            public string GetButton(bool IsWorking)
            {
                if (Type != "function") throw new InvalidCastException($"{Type}型は実行できません。");
                if (IsWorking) return FunctionWorking?? FunctionReady;
                else return FunctionReady;
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

            public double GetDisplayMag()
            {
                return Type switch
                {
                    "long" => DisplayMag,
                    _ => throw new ArgumentException("タイプが一致しません。"),
                };
            }

            string _type="Unknown";
            string _name="";
            string _title = "";
            string _unitName = "";
            string _description = "";

            /// <summary>
            /// 関数の型名
            /// </summary>
            public string Type
            {
                get => _type; 
                private set => _type = value; 
            }
            /// <summary>
            /// 関数の名前
            /// </summary>
            public string Name
            {
                get => _name; 
                private set => _name = value;
            }
            /// <summary>
            /// 関数の表示する文字列
            /// </summary>
            public string Title
            {
                get => _title;
                private set => _title = value;
            }
            /// <summary>
            /// 数量名の設定<br/>
            /// long型以外は例外がスローされます。
            /// </summary>
            public string UnitName
            {
                get { if (Type != "long") throw new InvalidCastException($"\"{Type}\"longに変換できません。");
                    return _unitName; } 
                private set => _unitName = value; 
            }
            /// <summary>
            /// 説明
            /// </summary>
            public string Description
            {
                get => _description;
                private set => _description = value;
            }
            public object Min
            {
                get
                {
                    return Type switch
                    {
                        "long" => (object)LongMin,
                        _ => throw new ArgumentException("タイプが一致しません。"),
                    };
                }
                private set
                {
                    switch(Type)
                    {
                        case "long":
                            if (value.GetType() == typeof(long))
                            {
                                if ((long)value > LongMax) throw new ArgumentOutOfRangeException($"値は必ずMaxより小さければなりません。 {(long)value}<={LongMax}");
                                LongMin = (long)value;
                            }else if(value.GetType() == typeof(int))
                            {
                                if ((int)value > LongMax) throw new ArgumentOutOfRangeException($"値は必ずMaxより小さければなりません。 {(long)value}<={LongMax}");
                                LongMin = (int)value;
                            }
                            else
                            {
                                throw new InvalidCastException($"{value.GetType()}は整数型に変換できません。");
                            }
                        break;
                        default: throw new ArgumentException("タイプが一致しません。");
                    }
                }
            }
            public object Max
            {
                get
                {
                    return Type switch
                    {
                        "long" => (object)LongMax,
                        _ => throw new ArgumentException("タイプが一致しません。"),
                    };
                }
                private set
                {
                    switch (Type)
                    {
                        case "long":
                            if (value.GetType() == typeof(long))
                            {
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"値は必ずMinより大きければなりません。 {(long)value}>={LongMin}");
                                LongMax = (long)value;
                            }
                            else if (value.GetType() == typeof(int))
                            {
                                if ((int)value < LongMin) throw new ArgumentOutOfRangeException($"値は必ずMinより大きければなりません。 {(long)value}>={LongMin}");
                                LongMax = (int)value;
                            }
                            else
                            {
                                throw new InvalidCastException($"{value.GetType()}は整数型に変換できません。");
                            }
                            break;
                        default: throw new ArgumentException("タイプが一致しません。");
                    }
                }
            }
            /// <summary>
            /// デフォルト値の取得
            /// </summary>
            public object Default
            {
                get
                {
                    return Type switch
                    {
                        "long" => LongDefault,
                        "string" => StringDefault,
                        "bool" => BooleanDefault,
                        "combo"=> LongDefault,
                        _ => throw new ArgumentException("タイプが一致しません。"),
                    };
                }
                private set
                {
                    switch (Type)
                    {
                        case "long":
                            if (value.GetType() == typeof(long))
                            {
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"値は必ずMinより大きければなりません。 {(long)value}>={LongMin}");
                                LongDefault = (long)value;
                            }
                            else if (value.GetType() == typeof(int))
                            {
                                LongDefault = (int)value;
                            }
                            else
                            {
                                throw new InvalidCastException($"{value.GetType()}は整数型に変換できません。");
                            }
                            break;
                        default: throw new ArgumentException("タイプが一致しません。");
                    }
                }
            }
            public string[] Items { get
                {
                    if (Type != "combo") throw new ArgumentException("タイプが一致しません");
                    string[] strings = new string[ComboStrings.Count];
                    for (int i = 0; i < ComboStrings.Count; i++) strings[i] = ComboStrings[i];
                    return strings;
                } }
            long LongValue;
            long LongMin;
            long LongMax;
            long LongDefault;
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
            readonly List<string> ComboStrings = new();
        }
    }
}