using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.Windows.ApplicationModel.Activation;
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
                for (int i = 0; i < Configs.Data.Count; i++)
                {
                    var config = Configs.Data[i].Setting;
                    for (int j = 0; j < config.Count; j++) sw.Write($"{config[j].GetConfigString()}");
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
                            SetConfigValue(lines[0], lines[1]);

                            Log.Instance.Debug($"{line}");
                            PASS++;
                        }
                        catch (Exception ex)
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
            SetValue("API_EEW_Delay", (e) => { api.EEW.Config.Delay = (int)(long)e;});
            SetValue("API_EEW_DelayDetectMode", (e) => { api.EEW.Config.DelayDetectMode = (int)(long)e;});
            SetValue("API_EEW_DelayDetectCoolDown", (e) => { api.EEW.Config.DelayDetectCoolDown = (int)(long)e;});
            SetValue("API_EEW_DMDATA_OnlyWarn", (e) => { api.EEW.Config.IsWarningOnlyInDMDATA = (bool)e;});
            SetValue("API_EQInfo_Delay", (e) => { api.EQInfo.Config.Delay = (uint)(long)e; });
            SetValue("API_EQInfo_Limit", (e) => { api.EQInfo.Config.Limit = (uint)(long)e; });
            SetValue("API_K-moni_Delay", (e) => { api.KyoshinAPI.Config.KyoshinDelayTime = (int)(long)e; });
            SetValue("API_K-moni_Frequency", (e) => { api.KyoshinAPI.Config.KyoshinFrequency = (int)(long)e; });
            SetValue("API_K-moni_Adjust", (e) => { api.KyoshinAPI.Config.AutoAdjustKyoshinTime = (int)(long)e*60; });
            SetValue("API_J-ALERT_Delay", (e) => { api.Jalert.Config.Delay = (uint)(long)e; });
            SetValue("GUI_Popup_J-ALERT", (e) => { api.Jalert.Config.IsDisplay = (bool)e; });
            var gui = APIs.GetInstance().KyoshinAPI.Config;
            SetValue("USER_Pos_Long", (e) => { gui.UserLong = (double)(long)e / 10000.0; });
            SetValue("USER_Pos_Lat", (e) => { gui.UserLat = (double)(long)e / 10000.0; });
            SetValue("USER_Pos_Display", (e) => { gui.UserDisplay = (bool)e; });
            var snd = Sound.Sounds.GetInstance().Config;

            SetValue("Sound_Volume_EEW", (e) => { snd.EEWVolume = (int)(long)e; });
            SetValue("Sound_Volume_Earthquake", (e) => { snd.EarthquakeVolume = (int)(long)e; });
            SetValue("Sound_Volume_Tsunami", (e) => { snd.TsunamiVolume = (int)(long)e; });
            SetValue("Sound_All_Mute", (e) => { snd.IsMute = (bool)e; });
            var tray = GUI.TrayHub.GetInstance()?.ConfigData;
            if (tray != null)
            {
                SetValue("GUI_Popup_EEW_Compact", (e) => { tray.IsWakeSimpleEEW = (bool)e; });
                SetValue("GUI_TopMost_EEW_Compact", (e) => { tray.IsTopSimpleEEW = (bool)e; });
                SetValue("Notification_EEW_Nationwide", (e) => { tray.NoticeNationWide = Struct.ConfigBox.Notification_EEW_Nationwide.GetIndex((int)e); });
                SetValue("Notification_EEW_Area", (e) => { tray.NoticeArea = Struct.ConfigBox.Notification_EEW_Area.GetIndex((int)e); });
            }
            SetValue("Notification_Popup_Notify", (e) => { ToastNotification.IsNewNotification = (bool)e; });
#if DEBUG||ADMIN
            SetValue("Twitter_Enable_Tweet", (e) => { Twitter.APIs.GetInstance().Config.TweetEnabled = (bool)e; });
            SetValue("Twitter_J-ALERT_Tweet", (e) => { Twitter.APIs.GetInstance().Config.IsTweetJ_ALERT = (bool)e; });


            SetValue("Misskey_Enable_Note", (e) => { Misskey.APIData.Config.IsEnableEarthquakeNote = (bool)e; });
            SetValue("Misskey_J-ALERT_Note", (e) => { Misskey.APIData.Config.IsEnableJAlertNote = (bool)e; });
            SetValue("Misskey_EEW_Delay", (e) => { MisakiEQ.Funcs.Misskey.GetInstance().EEWDelayTime = (int)(long)e; });
            SetValue("Misskey_EEW_Delay_IsInter", (e) => { MisakiEQ.Funcs.Misskey.GetInstance().IsInterSend = (bool)e; });
            SetValue("Discord_WebHook_Enable", (e) => { MisakiEQ.Lib.Discord.WebHooks.Main.Instance.Config.EnablePost = (bool)e; });
#endif
        }
        private bool SetValue(string id,Action<object> act)
        {
            try
            {
                var c = GetConfigClass(id);
                if (c == null)
                {
                    Log.Instance.Error($"「{id}」は存在しません。");
                    return false;
                }
                if (c.GetType() == typeof(LongIndexData)) act(((LongIndexData)c).Value);
                else if (c.GetType() == typeof(StringIndexData)) act(((StringIndexData)c).Value);
                else if (c.GetType() == typeof(BoolIndexData)) act(((BoolIndexData)c).Value);
                else if (c.GetType() == typeof(ComboIndexData)) act(((ComboIndexData)c).Value);
                else return false;
                return true;
            }catch(Exception ex)
            {
                Log.Instance.Error($"「{id}」は読み込めませんでした。理由:{ex.Message}");
                return false;
            }
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
                    if (config[j] is ComboIndexData)
                    {
                        ConfigTemplates.Add(new(config[j].Name, $"={((ComboIndexData)config[j]).Value}"));
                    }
                    else
                    if (config[j].ValueDefaultable()) ConfigTemplates.Add(new(config[j].Name, $"{config[j].GetValue()}"));
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
        string GetConfigValue(string name)
        {
            var cls = GetConfigClass(name);
            if (cls == null) return string.Empty;
            return cls.GetConfigString();
        }

        /// <summary>
        /// 対応する名前のコンフィグに値を書き込みます。
        /// </summary>
        /// <param name="name">値を設定したい名前</param>
        /// <param name="value">設定する値</param>
        /// <param name="IsThrow">存在しない時に例外をスローするか</param>
        /// <exception cref="ArgumentNullException"></exception>
        void SetConfigValue(string name, string value, bool IsThrow = true)
        {
            var cls = GetConfigClass(name);
            if (cls == null)
            {
                if (IsThrow) throw new ArgumentException($"\"{name}\"はConfig上に存在しません。");
                return;
            }
            cls.SetConfigString(value);
        }
        private class ConfigTemplate
        {
            public ConfigTemplate(string key, string value)
            {
                Key = key;
                Value = value;
            }
            public string Key = string.Empty;
            public string Value = string.Empty;
        }
        readonly List<ConfigTemplate> ConfigTemplates = new();
        public void DiscardConfig()
        {
            Log.Instance.Debug($"現在の設定が保存せずに破棄された為、コンフィグを前の状態に戻します。");
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < ConfigTemplates.Count; i++) SetConfigValue(ConfigTemplates[i].Key, ConfigTemplates[i].Value, false);
            sw.Stop();
            Log.Instance.Debug($"コンフィグを前の状態に戻しました。処理時間 : {sw.Elapsed}");
        }
        public Cfg Configs = new();
        public class Cfg
        {
            public readonly List<ConfigGroup> Data = new();
            public void OutputLog()
            {
                int i = 0;
                foreach(var d in Data)
                {
                    foreach(var e in d.Setting)
                    {
                        i++;
                        try
                        {
                            Log.Instance.Debug($"No.{i,4}: \"{d.Name}\".\"{e.Name}\" Value=\"{e.GetValue()}\"");
                        }
                        catch  { }
                    }
                }
            }
            public Cfg()
            {
                GetGroup("通信設定", true)?.Add(new LongIndexData("API_EEW_Delay", "EEW標準遅延", unitName: "秒", displayMag: 1000, description: "標準状態の緊急地震速報の遅延時間です。", min: 1000, max: 5000, def: 1000));   //通常時の遅延(ms)
                GetGroup("通信設定", true)?.Add(new LongIndexData("API_EEW_DelayDetectMode", "EEW検知遅延", unitName: "秒", displayMag: 1000, description: "検知状態の緊急地震速報の遅延時間です。", min: 150, max: 5000, def: 200));     //検出時の遅延(ms)
                GetGroup("通信設定", true)?.Add(new LongIndexData("API_EEW_DelayDetectCoolDown", "EEWモード移行時間", unitName: "秒", displayMag: 1000, description: "検知状態から標準状態に回復する時間です。", min: 1000, max: 10000, def: 4000));     //検出から通常時に戻る時間(ms)
                GetGroup("通信設定", true)?.Add(new BoolIndexData("API_EEW_DMDATA_OnlyWarn", "DMDATA取得モード", "警報のみ受け取るか、予報も受け取るかを選択できます。", def: false, "警報のみ取得", "予報・警報を取得"));
                GetGroup("通信設定", true)?.Add(new LongIndexData("API_EQInfo_Delay", "地震情報遅延", unitName: "秒", displayMag: 1000, description: "地震情報全般の取得遅延時間です。\n地震と津波情報共通でリクエスト多過ぎるとエラー出ます。", min: 2000, max: 30000, def: 3000));   //通常時の遅延(ms)
                GetGroup("通信設定", true)?.Add(new LongIndexData("API_EQInfo_Limit", "地震情報取得項目数", description: "1回で地震情報を取得する数です。\n値を大きくすると1回あたりより多くの情報が更新されますが、その分遅くなります。", min: 1, max: 100, def: 10));   //取得時の配列の数
                GetGroup("通信設定", true)?.Add(new LongIndexData("API_K-moni_Delay", "強震モニタ遅延時間", description: "強震モニタの時刻からの遅延を設定できます。\n低い程低遅延ですが、更新されない可能性があります。", min: 0, max: 5, def: 1, unitName: "秒"));   //取得時の配列の数
                GetGroup("通信設定", true)?.Add(new LongIndexData("API_K-moni_Frequency", "強震モニタ更新間隔", description: "強震モニタの更新間隔です。データ消費量を抑えたい時にお使いください。", min: 1, max: 5, def: 1, unitName: "秒"));   //取得時の配列の数
                GetGroup("通信設定", true)?.Add(new LongIndexData("API_K-moni_Adjust", "強震モニタ補正間隔", description: "強震モニタの時刻調整間隔です。自動で時刻補正する間隔を設定できます。", min: 10, max: 720, def: 30, unitName: "分"));   //取得時の配列の数
                GetGroup("通信設定", true)?.Add(new LongIndexData("API_J-ALERT_Delay", "Jアラート遅延時間", description: "Jアラートの更新間隔です。", min: 3, max: 60, def: 30, unitName: "秒"));
                GetGroup("ユーザー設定", true)?.Add(new LongIndexData("USER_Pos_Lat", "所在地(緯度)", description: "ユーザーの緯度です。予測震度を表示させたい場合にお使いください。", min: 237000, max: 462000, def: 356896, displayMag: 10000));   //取得時の配列の数
                GetGroup("ユーザー設定", true)?.Add(new LongIndexData("USER_Pos_Long", "所在地(経度)", description: "ユーザーの経度です。予測震度を表示させたい場合にお使いください。", min: 1225000, max: 1460000, def: 1396983, displayMag: 10000));   //取得時の配列の数
                GetGroup("ユーザー設定", true)?.Add(new ReadonlyIndexData("USER_Pos_Result", "該当地域名", "緯度・経度に対応されるであろう該当地域名です。"));
                GetGroup("ユーザー設定", true)?.Add(new BoolIndexData("USER_Pos_Display", "強震モニタに座標表示", description: "ユーザーの経度経度情報を強震モニタに紫色で表示します。", def: false, "強震モニタに表示", "強震モニタに非表示"));   //取得時の配列の数
                GetGroup("通信設定", true)?.Add(new FunctionIndexData("Kyoshin_Time_Adjust", "強震モニタ時刻調整", description: "強震モニタの時刻調整を実行します。", "時刻調整実行", workingTitle: "時刻調整中...", action: new Action(async () => { await APIs.GetInstance().KyoshinAPI.FixKyoshinTime(); })));//取得時の配列の数
                GetGroup("通信設定", true)?.Add(new ReadonlyIndexData("Kyoshin_Time", "強震モニタ時刻", "強震モニタ上の最終更新時刻です。"));
                GetGroup("Project DM-Data Service", true)?.Add(new FunctionIndexData("DMDATA_AuthFunction", "アプリ連携", "MisakiEQでDMDATAの緊急地震速報データを取得できるようにします。", "認証", workingTitle: "ブラウザで認証中..."));
#if ADMIN||DEBUG
                GetGroup("SNS設定", true)?.Add(new FunctionIndexData("Twitter_Auth", "Twitter認証", "アカウント認証します", "認証", workingTitle: "認証中..."));
                GetGroup("SNS設定", true)?.Add(new ReadonlyIndexData("Twitter_Auth_Info", "Twitter認証情報", ""));
                GetGroup("SNS設定", true)?.Add(new ReadonlyIndexData("Twitter_Auth_UserID", "ユーザーID", ""));
                GetGroup("SNS設定", true)?.Add(new ReadonlyIndexData("Twitter_Auth_UserName", "ユーザー名", ""));
                GetGroup("SNS設定", true)?.Add(new ReadonlyIndexData("Twitter_Auth_Tweet", "ツイート数", ""));
                GetGroup("SNS設定", true)?.Add(new ReadonlyIndexData("Twitter_Auth_Follower", "フォロワー数", ""));
                GetGroup("SNS設定", true)?.Add(new BoolIndexData("Twitter_Enable_Tweet", "自動ツイートの有効化", "自動でユーザーに地震情報をツイートします", def: true, "Twitterへの自動ツイートが有効", "Twitterへの自動ツイートが無効"));
                GetGroup("SNS設定", true)?.Add(new BoolIndexData("Twitter_J-ALERT_Tweet", "J-ALERT配信有効化", "自動でユーザーにJアラートをツイートします", def: false, "Twitterへの自動ツイートが有効", "Twitterへの自動ツイートが無効"));
                GetGroup("SNS設定", true)?.Add(new BoolIndexData("Misskey_Enable_Note", "自動ノートの有効化", "自動でユーザーに地震情報をMisskeyにノートします。", def: false, "Misskeyへの自動ノートが有効", "Misskeyへの自動ノートが無効"));
                GetGroup("SNS設定", true)?.Add(new BoolIndexData("Misskey_J-ALERT_Note", "J-ALERT配信有効化", "自動でユーザーに地震情報をMisskeyにノートします。", def: false, "Misskeyへの自動ノートが有効", "Misskeyへの自動ノートが無効"));
                GetGroup("SNS設定", true)?.Add(new LongIndexData("Misskey_EEW_Delay", "EEWノート遅延", unitName: "秒", displayMag: 1000, description: "遅延時間です。", min: 0, max: 10000, def: 2000));   //通常時の遅延(ms)
                GetGroup("SNS設定", true)?.Add(new BoolIndexData("Misskey_EEW_Delay_IsInter", "遅延の割り込み", "パブリック投稿時に割り込んで投稿します。", def: false, "割り込み処理が有効", "割り込み処理が無効"));
                GetGroup("SNS設定", true)?.Add(new BoolIndexData("Discord_WebHook_Enable", "Discord WebHook", "Discordのチャットに投稿します", def: false, "WebHook投稿が有効", "WebHook投稿が無効"));

#endif
                GetGroup("サウンド設定", true)?.Add(new LongIndexData("Sound_Volume_EEW", "EEWの通知音量", "緊急地震速報発生時に通知される音量を設定します。", def: 100, min: 0, max: 100, unitName: "%"));
                GetGroup("サウンド設定", true)?.Add(new LongIndexData("Sound_Volume_Earthquake", "地震情報の通知音量", "地震情報発表時に通知される音量を設定します。", def: 100, min: 0, max: 100, unitName: "%"));
                GetGroup("サウンド設定", true)?.Add(new LongIndexData("Sound_Volume_Tsunami", "津波情報の通知音量", "津波情報発表時に通知される音量を設定します。", def: 100, min: 0, max: 100, unitName: "%"));
                GetGroup("サウンド設定", true)?.Add(new BoolIndexData("Sound_All_Mute", "完全ミュート", "音声デバイスがない時などはこのチェックボックスを有効にすることで軽量化されます。", def: false, "有効(再生されません)", "無効"));

                GetGroup("緊急地震速報発表時", true)?.Add(new BoolIndexData("GUI_Popup_EEW_Compact", "簡易情報のポップ表示", "緊急地震速報が発令されると簡易ウィンドウがポップアップされます。", true, "ポップアップ表示", "ポップアップ非表示"));
                GetGroup("緊急地震速報発表時", true)?.Add(new BoolIndexData("GUI_TopMost_EEW_Compact", "簡易情報の表示モード", "簡易ウィンドウが前面表示されます。", true, "前面表示", "標準表示"));
                GetGroup("J-ALERT", true)?.Add(new BoolIndexData("GUI_Popup_J-ALERT", "全画面ポップアップ", "Jアラート発令時に自動的に全画面でポップアップ表示されます。", true, "表示する", "表示しない"));
                GetGroup("アプリ情報", true)?.Add(new ReadonlyIndexData("AppInfo_Uptime", "アプリ起動時間", "起動してからの経過時間です。"));
                GetGroup("アプリ情報", true)?.Add(new ReadonlyIndexData("AppInfo_UsingAPI", "使用中のEEW API", "EEW APIの使用状況"));
                GetGroup("通知設定", true)?.Add(new ComboIndexData("Notification_EEW_Nationwide", "EEW全国通知条件", "全国共通で緊急地震速報を通知する条件を設定します", 9, new string[] { "7", "≧6+", "≧6-", "≧5+", "≧5-", "≧4", "≧3", "≧2", "≧1", "ALL", "Warning only", "None" }));
                GetGroup("通知設定", true)?.Add(new ComboIndexData("Notification_EEW_Area", "EEW地域通知条件", "お住まいの地域で緊急地震速報を通知する条件を設定します", 8, new string[] { "7", "≧6+", "≧6-", "≧5+", "≧5-", "≧4", "≧3", "≧2", "≧1", "≧0", "None" }));
                GetGroup("通知設定", true)?.Add(new BoolIndexData("Notification_Popup_Notify", "トースト通知タイプ", "通知タイプを設定します。", def: false, "Windows プッシュ通知", "Windows Form標準通知"));
                GetGroup("Debug Mode", true)?.Add(new StringIndexData("Debug_Input", "Misskey", "デバック用", ""));
                GetGroup("Debug Mode", true)?.Add(new FunctionIndexData("Debug_Function", "ノート投稿", "デバック用", "Execute", workingTitle: "Working..."));

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
                var instance = Data.Find(a => a.Name == name);
                if (instance == null)
                {
                    if (Create)
                    {
                        Data.Add(new(name));
                        return Data[^1].Setting;
                    }
                    else return null;
                }
                else
                    return instance.Setting;
            }
            public class ConfigGroup
            {
                public ConfigGroup(string name)
                {
                    _name = name;
                }

                readonly string _name = "";
                public string Name { get => _name; }
                public readonly List<IndexData> Setting = new();
            }
        }
        public abstract class IndexData
        {
            public Action<object>? ApplyAction = null; 
            public EventHandler? ValueChanged = null;
            public string Name { get; }
            public string Title { get; }
            public string Description { get; }
            protected internal IndexData(string name, string title, string description)
            {
                Name = name;
                Title = title;
                Description = description;
            }
            /// <summary>
            /// 指定したクラスのコンフィグに値を入れる予定(未実装)
            /// </summary>
            /// <returns></returns>
            public virtual bool Apply()
            {
                return false;
            }
            /// <summary>
            /// Configに保存するとき用の文字列に変換
            /// </summary>
            /// <returns></returns>
            public virtual string GetConfigString()
            {
                return string.Empty;
            }
            /// <summary>
            /// Configから読み出すとき用の関数
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public virtual bool SetConfigString(string value)
            {
                return false;
            }
            /// <summary>
            /// デフォルト値が使用できるか
            /// </summary>
            /// <returns></returns>
            public virtual bool ValueDefaultable()
            {
                return false;
            }
            /// <summary>
            /// 現在の値をStringで取得
            /// </summary>
            /// <returns></returns>
            public virtual string GetValue()
            {
                return string.Empty;
            }
            /// <summary>
            /// 値を設定する
            /// </summary>
            /// <param name="value"></param>
            public virtual void SetValue(object value)
            {
            }
            // 共通のメソッドやプロパティを定義する場合はここに追加する
        }
        public class LongIndexData : IndexData
        {
            public long Min { get; set; }
            public long Max { get; set; }
            public long Default { get; set; }
            private long _value;
            public long Value
            {
                get { return _value; }
                set
                {
                    if (value < Min) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} < {Min}");
                    if (value > Max) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} > {Max}");
                    if (_value != value)
                    {
                        _value = value;
                        ValueChanged?.Invoke(this, EventArgs.Empty);
                    }
                }

            }
            public string UnitName { get; set; }
            public double DisplayMag { get; set; }

            public LongIndexData(string name, string title, string description, long min = long.MinValue, long max = long.MaxValue, long def = 0, string unitName = "", double displayMag = 1)
                : base(name, title, description)
            {
                Min = min;
                Max = max;
                Default = def;
                _value = def;
                UnitName = unitName;
                DisplayMag = displayMag;
            }

            public override string GetConfigString()
            {
                return Name + "=" + Value.ToString() + Environment.NewLine;
            }

            public override bool SetConfigString(string value)
            {
                long val = long.Parse(value);
                if (val < Min) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} < {Min}");
                if (val > Max) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} > {Max}");
                Value = val;
                return true;
            }
            public override bool ValueDefaultable()
            {
                return true;
            }
            public override string GetValue()
            {
                return Value.ToString();
            }
            public override void SetValue(object value)
            {
                if (value is long || value is int || value is short)
                {
                    Value = (long)value;
                }
                else throw new InvalidCastException($"{value}は整数型ではありません。Type:{value.GetType()}");
            }
            public override bool Apply()
            {
                if (ApplyAction == null) return false;
                try
                {
                    ApplyAction(Value);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Instance.Error($"{ex.Message}");
                }
                return false;
            }
            // 必要に応じて派生クラス固有のメソッドやプロパティを追加する
        }
        public class StringIndexData : IndexData
        {
            public int MaxLength { get; set; }
            public string DefaultValue { get; set; }
            string _value;
            public string Value
            {
                get => _value; set
                {
                    if (value.Length > MaxLength) throw new InvalidDataException("value is out of max length.");
                    if (!string.Equals(_value, value))
                    {
                        _value = value;
                        ValueChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            public StringIndexData(string name, string title, string description, string defaultValue = "", int maxLength = int.MaxValue)
                : base(name, title, description)
            {
                MaxLength = maxLength;
                DefaultValue = defaultValue;
                _value = defaultValue;
            }
            public override string GetConfigString()
            {
                return Name + "=" + Value.Replace("%", "%%").Replace("=", "%3D").Replace("\n", "%0D") + Environment.NewLine;
            }
            public override bool SetConfigString(string value)
            {
                string? str = (string?)value;
                if (string.IsNullOrEmpty(str)) str = string.Empty;
                Value = str.Replace("%0D", "\n").Replace("%3D", "=").Replace("%%", "%");
                return true;
            }
            public override bool ValueDefaultable()
            {
                return true;
            }
            public override string GetValue()
            {
                return Value.ToString();
            }
            public override void SetValue(object value)
            {
                if (value is string)
                {
                    Value = (string)value;
                }
                else throw new InvalidCastException($"{value}はstring型ではありません。Type:{value.GetType()}");
            }
            public override bool Apply()
            {
                if (ApplyAction == null) return false;
                try
                {
                    ApplyAction(Value);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Instance.Error($"{ex.Message}");
                }
                return false;
            }
            // 必要に応じて派生クラス固有のメソッドやプロパティを追加する
        }
        public class BoolIndexData : IndexData
        {
            public bool DefaultValue { get; }
            private bool _value;
            public bool Value
            {
                get => _value; set
                {
                    if (_value != value)
                    {
                        _value = value;
                        ValueChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            public string BoolToggleOn { get; set; }
            public string BoolToggleOff { get; set; }

            public BoolIndexData(string name, string title, string description = "", bool def = false, string toggleOnText = "", string toggleOffText = "")
                : base(name, title, description)
            {
                DefaultValue = def;
                _value = def;
                BoolToggleOn = toggleOnText;
                BoolToggleOff = toggleOffText;
            }
            public string GetToggleText(bool Is)
            {
                return Is ? BoolToggleOn : BoolToggleOff;
            }
            public override string GetConfigString()
            {
                return Name + "=" + Value.ToString() + Environment.NewLine;
            }
            public override bool SetConfigString(string value)
            {
                if (value.ToLower() == "true" || value == "0")
                {
                    Value = true;
                }
                else if (value.ToLower() == "false" || value == "1")
                {
                    Value = false;
                }
                else
                {
                    throw new ArgumentException($"bool型ではない、もしくは正しく検出できませんでした。val=\"{value}\"", nameof(value));
                }
                return true;
            }
            public override bool ValueDefaultable()
            {
                return true;
            }
            public override string GetValue()
            {
                return Value.ToString();
            }

            public override void SetValue(object value)
            {
                if (value is bool)
                {
                    Value = (bool)value;
                }
                else throw new InvalidCastException($"{value}はboolean型ではありません。Type:{value.GetType()}");
            }
            public override bool Apply()
            {
                if (ApplyAction == null) return false;
                try
                {
                    ApplyAction(Value);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Instance.Error($"{ex.Message}");
                }
                return false;
            }
        }
        // 必要に応じて派生クラス固有のメソッドやプロパティを追加する

        public class FunctionIndexData : IndexData
        {
            private Action? FunctionAction { get; set; }
            public string FunctionReady { get; set; }
            public string? FunctionWorking { get; set; }

            public FunctionIndexData(string name, string title, string description, string readyTitle, Action? action = null, string? workingTitle = null)
                : base(name, title, description)
            {
                FunctionAction = action;
                FunctionReady = readyTitle;
                FunctionWorking = workingTitle;
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
                FunctionAction = act;
            }

            /// <summary>
            /// Function関数の場合は関数が実行されます。<br/>
            /// そうでない場合は例外がスローされます。
            /// </summary>
            /// <exception cref="InvalidCastException"></exception>
            public void ExecuteAction()
            {
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
                    return __buttonEnable;
                }
                set
                {
                    if (__buttonEnable != value)
                    {
                        __buttonEnable = value;
                        try
                        {
                            ButtonChanged?.Invoke(this, EventArgs.Empty);
                        }
                        catch (Exception ex)
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
                if (IsWorking) return FunctionWorking ?? FunctionReady;
                else return FunctionReady;
            }
            // 必要に応じて派生クラス固有のメソッドやプロパティを追加する
        }
        public class ReadonlyIndexData : IndexData
        {
            string _value;
            public string Value { get=>_value; set
                {
                    if (!string.Equals(value, _value))
                    {
                        _value = value;
                        ValueChanged?.Invoke(this, EventArgs.Empty);
                    }
                } }
            public ReadonlyIndexData(string name, string title, string description)
                : base(name, title, description)
            {
                _value = "";
            }

            public override string GetValue()
            {
                return Value.ToString();
            }
            public override void SetValue(object value)
            {
                if (value is string)
                {
                    Value = (string)value;
                }
                else throw new InvalidCastException($"{value}はstring型ではありません。Type:{value.GetType()}");
            }

            // 必要に応じて派生クラス固有のメソッドやプロパティを追加する
        }
        public class ComboIndexData : IndexData
        {
            public List<string> ComboStrings { get; set; }
            public int Default { get; set; }
            public int Value { get; set; }
            public ComboIndexData(string name, string title, string description, int def, IEnumerable<string> list)
                : base(name, title, description)
            {
                ComboStrings = new();
                Default = def;
                Value = def;

                foreach (var item in list)
                {
                    ComboStrings.Add(item);
                }
            }

            public override string GetConfigString()
            {
                return Name + "=" + ComboStrings[Value].Replace("%", "%%").Replace("=", "%3D").Replace("\n", "%0D") + Environment.NewLine;
            }
            public override bool SetConfigString(string value)
            {
                if (value.StartsWith("="))
                {
                    if (int.TryParse(value.Replace("=", ""), out int num))
                    {
                        if (ComboStrings.Count > num && 0 <= num)
                        {
                            Value = num;
                            return true;
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
                        Value = i;
                        return true;
                    }

                    if (i == ComboStrings.Count - 1)
                        throw new ArgumentException($"\"{vals}\"は[{Name}]のコレクションの中には存在しませんでした。", nameof(value));
                }
                return false;
            }

            public override bool ValueDefaultable()
            {
                return true;
            }
            public string[] Items
            {
                get
                {
                    string[] strings = new string[ComboStrings.Count];
                    for (int i = 0; i < ComboStrings.Count; i++) strings[i] = ComboStrings[i];
                    return strings;
                }
            }
            // 必要に応じて派生クラス固有のメソッドやプロパティを追加する
        }
    }
}