﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using DmdataSharp;
using DmdataSharp.Authentication.OAuth;
using MisakiEQ.Background.API.EQInfo.JSON;
using MisakiEQ.Struct;

namespace MisakiEQ.Background.API.EEW.dmdata
{
    public class Analysis
    {
        DmdataApiClientBuilder ApiBuilder;
        DmdataV2ApiClient? Client = null;
        DmdataV2Socket? Socket = null;
        string RefreshToken = "";
        Struct.EEW TempData;
        public bool IsWarnOnly = false;
        public Analysis()
        {
            ApiBuilder = DmdataApiClientBuilder.Default
                .UserAgent("MisakiEQ")
                .Referrer(new Uri("https://github.com/Misaki0331/MisakiEQv2/"));
            TempData = new();
            TempData.Serial.Infomation = Struct.EEW.InfomationLevel.Default;
        }
        public async Task<string?> Authentication(CancellationToken? token)
        {
            try
            {
                var clientId = Resources.API.API.DMDataAPI;
                var scopes = new[] { "contract.list", "telegram.list", "socket.start", "gd.eew", "socket.close", "eew.get.warning", "eew.get.forecast" };
                var credential = await SimpleOAuthAuthenticator.AuthorizationAsync(
                    ApiBuilder.HttpClient,
                    clientId,
                    scopes,
                    "MisakiEQ",
                    u => Process.Start(new ProcessStartInfo("cmd", $"/c start {u.Replace("&", "^&")}") { CreateNoWindow = true }),
                    token: token);
                RefreshToken = credential.RefreshToken;
                Log.Debug($"RefreshToken : {RefreshToken}");
                return credential.RefreshToken;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        private enum LoadInfomation{
            LoadFailed,
            NotFound,
            Found
        }
        private LoadInfomation LoadToken()
        {
            Log.Debug("トークンを読み込みます。");
            if (!File.Exists("DMDataToken.cfg"))
            {
                Log.Warn("トークンファイルが存在しません。");
                return LoadInfomation.NotFound;
            }
            try
            {
                using var reader = new StreamReader("DMDataToken.cfg");
                RefreshToken = reader.ReadToEnd();
                return LoadInfomation.Found;
            }
            catch (Exception ex)
            {
                Log.Error($"読込中にエラー : {ex.Message}");
                return LoadInfomation.LoadFailed;
            }
        }
        public bool Init()
        {
            try
            {
                if (string.IsNullOrEmpty(RefreshToken))
                {
                    var res = LoadToken();
                    if (res != LoadInfomation.Found) return false;
                }
                var clientId = Resources.API.API.DMDataAPI;
                var scopes = new[] { "contract.list", "telegram.list", "socket.start", "gd.eew", "socket.close", "eew.get.warning", "eew.get.forecast"};
                var credential = new OAuthRefreshTokenCredential(ApiBuilder.HttpClient,
                        scopes,
                        clientId,
                        RefreshToken);
                ApiBuilder = ApiBuilder.UseOAuth(credential);
                Client = ApiBuilder.BuildV2ApiClient();
            } catch (Exception e)
            {
                Log.Error(e);
                return false;
            }
            return true;
            /*var telegramList = await Client.GetEewEventsAsync(limit: 10);
            Log.Instance.Debug($"Status : {telegramList.Status}");
            for(int i=0;i< telegramList.Items.Length; i++)
            {
                string str = $"Data[{i}] : {telegramList.Items[i].DateTime} {(telegramList.Items[i].IsCanceled ? "Cancelled" : "")} ";
                var data = telegramList.Items[i].Earthquake;
                if (data != null)
                {
                    str += $"{data.Hypocenter.Name} M{data.Magnitude.Value:0.0} {data.Hypocenter.Depth.Value}km";
                }
                Log.Instance.Debug($"Data[{i}] : {str}");
            }*/
        }
        public void APIClose()
        {
            if (Socket != null && !Socket.IsDisposed) Socket?.Dispose();
            Socket = null;
            Client?.Dispose();
            Client = null;
        }
        bool EndTask = false;
        public event EventHandler<EEWEventArgs>? UpdateHandler = null;
        public async Task Loop(CancellationToken token)
        {
            try
            {
                SocketConnection();
                while (true)
                {
                    await Task.Delay(1000, token);
                    if (!(Socket != null && Socket.IsConnected)) Log.Warn("ソケットは接続されていません。");
                    token.ThrowIfCancellationRequested();
                }
            }
            catch (TaskCanceledException)
            {
                Log.Info("ソケットを中断します。");
                EndTask = true;
                if (Socket != null) await Socket.DisconnectAsync();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        private void SocketConnected(object? sender, DmdataSharp.WebSocketMessages.V2.StartWebSocketMessage? e)
        {
            if (e != null)
            {
                string types = string.Empty;
                for (int i = 0; i < e.Classifications.Length; i++) types += $"{e.Classifications[i]} ";
                Log.Info($"ソケット接続完了 {e.Time} 受け取る配信内容:{types}");
            }
            else
                Log.Warn("ソケットが接続されましたがデータはnullです。");
        }
        private void SocketDisconnected(object? sender, EventArgs? e)
        {
            if (Socket != null && !Socket.IsDisposed) Socket.Dispose();
            if (EndTask) Log.Debug("ソケットは正常に切断されました。");
            else
            {
                Log.Debug("ソケットが予期しない切断された為再接続します。");
                SocketConnection();
            }
        }
        private void SocketError(object? sender, DmdataSharp.WebSocketMessages.V2.ErrorWebSocketMessage? e)
        {
            string Error = "エラー内容はnullでした。";
            if (e != null)
            {
                Error = $"Err.{e.Code} {e.Error}";
                if (e.Close) Error += " ソケットを終了しました。";
            }
            Log.Error($"WebSocketでエラー発生 : {Error}");
        }
        private void SocketReceive(object? sender, DmdataSharp.WebSocketMessages.V2.DataWebSocketMessage? data)
        {
            if (data != null)
            {

                using var telegramStream = data.GetBodyStream();
                GetData(telegramStream);
                try
                {
                    if (!Directory.Exists("EEW_History/")) Directory.CreateDirectory("EEW_History");
                    using var sw = new StreamWriter($"EEW_History/{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.xml");
                    sw.Write(data.GetBodyString());
                } catch (Exception ex)
                {
                    Log.Error(ex);
                }

            }
            else Log.Error("WebSocketから受信しましたがデータがありませんでした。");
        }

        public void Test(string str)
        {
            Encoding encoding = Encoding.UTF8;
            GetData(new MemoryStream(encoding.GetBytes(str)));
        }

        public async void GetData(Stream stream)
        {
            try
            {
                XDocument document;
                XmlNamespaceManager nsManager;
                using (var reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true }))
                {
                    document = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);
                    nsManager = new XmlNamespaceManager(reader.NameTable);
                }
                nsManager.AddNamespace("jmx", "http://xml.kishou.go.jp/jmaxml1/");
                // 地震情報の場合以下の追記が必要
                nsManager.AddNamespace("eb", "http://xml.kishou.go.jp/jmaxml1/body/seismology1/");
                nsManager.AddNamespace("jmx_eb", "http://xml.kishou.go.jp/jmaxml1/elementBasis1/");

                // XPathを使用して電文のタイトルが取得できる
                if (document.Root != null)
                {
                    var root = document.Root;
                    Struct.EEW eew = new();
                    //地震識別番号
                    string? tmp = root.XPathSelectElement($"/jmx:Report/{GetName("Head")}/{GetName("EventID")}", nsManager)?.Value;
                    if (tmp != null) eew.Serial.EventID = tmp; else eew.Serial.EventID = "Unknown";
                    //情報番号
                    if (!int.TryParse(root.XPathSelectElement($"/jmx:Report/{GetName("Head")}/{GetName("Serial")}", nsManager)?
                        .Value, out eew.Serial.Number)) eew.Serial.Number = 0;
                    //最終報チェック
                    tmp = root.XPathSelectElement($"/jmx:Report/{GetName("Body")}/{GetName("NextAdvisory")}", nsManager)?.Value;
                    if (tmp != null && tmp.Contains("最終報")) eew.Serial.IsFinal = true; else eew.Serial.IsFinal = false;


                    var info = root.XPathSelectElement($"/jmx:Report/{GetName("Control")}/{GetName("Title")}", nsManager)?.Value;
                    switch (info)
                    {
                        case "緊急地震速報（予報）":
                            eew.Serial.Infomation = Struct.EEW.InfomationLevel.OldForecast;
                            return;
                        case "緊急地震速報（地震動予報）":
                            eew.Serial.Infomation = Struct.EEW.InfomationLevel.Forecast;
                            break;
                    }
                    //緊急地震速報のタイプ(キャンセル報か予報以上か)
                    tmp = root.XPathSelectElement($"/jmx:Report/{GetName("Head")}/{GetName("InfoType")}", nsManager)?.Value;
                    if (string.Equals(tmp,"取消")) eew.Serial.Infomation = Struct.EEW.InfomationLevel.Cancelled;
                    //発表時刻チェック
                    Log.Debug(root.XPathSelectElement($"/jmx:Report/{GetName("Head")}/{GetName("ReportDateTime")}", nsManager)?.Value ?? "");
                    if (!DateTime.TryParse(root.XPathSelectElement($"/jmx:Report/{GetName("Head")}/{GetName("ReportDateTime")}", nsManager)?.Value,
                        out eew.Serial.UpdateTime)) eew.Serial.UpdateTime = DateTime.MinValue;
                    //地震発生時刻
                    if (!DateTime.TryParse(root.XPathSelectElement($"/jmx:Report/{GetName("Body")}/{GetName("Earthquake")}/{GetName("OriginTime")}", nsManager)?.Value,
                        out eew.EarthQuake.OriginTime)) eew.EarthQuake.OriginTime = DateTime.MinValue;
                    //マグニチュードチェック
                    if (!double.TryParse(root.XPathSelectElement($"/jmx:Report/{GetName("Body")}/{GetName("Earthquake")}/jmx_eb:Magnitude", nsManager)?.Value,
                        out eew.EarthQuake.Magnitude)) eew.EarthQuake.Magnitude = double.NaN;
                    //震源地名
                    tmp = root.XPathSelectElement($"/jmx:Report/{GetName("Body")}/{GetName("Earthquake")}/{GetName("Hypocenter")}/{GetName("Area")}/{GetName("Name")}", nsManager)?.Value;
                    if (tmp != null) eew.EarthQuake.Hypocenter = tmp;
                    //震源コード
                    if (!int.TryParse(root.XPathSelectElement($"/jmx:Report/{GetName("Body")}/{GetName("Earthquake")}/{GetName("Hypocenter")}/{GetName("Area")}/{GetName("Code")}", nsManager)?.Value,
                        out eew.EarthQuake.HypocenterCode)) eew.EarthQuake.HypocenterCode = 0;
                    //震央データ取得
                    tmp = root.XPathSelectElement($"/jmx:Report/{GetName("Body")}/{GetName("Earthquake")}/{GetName("Hypocenter")}/{GetName("Area")}/jmx_eb:Coordinate", nsManager)?.Value;
                    if (tmp != null)
                    {
                        tmp = tmp.Replace("+", "/+").Replace("-", "/-");
                        tmp = tmp.Remove(0, 1);
                        tmp = tmp.Remove(tmp.Length - 1, 1);
                        var tmp2 = tmp.Split('/');
                        if (tmp2.Length == 3)
                        {
                            if (!double.TryParse(tmp2[0], out eew.EarthQuake.Location.Lat)) eew.EarthQuake.Location.Lat = double.NaN;
                            if (!double.TryParse(tmp2[1], out eew.EarthQuake.Location.Long)) eew.EarthQuake.Location.Long = double.NaN;
                            if (int.TryParse(tmp2[2], out eew.EarthQuake.Depth)) eew.EarthQuake.Depth /= (-1000); else eew.EarthQuake.Depth = 0;
                        }
                        else
                        {
                            eew.EarthQuake.Location.Lat = double.NaN;
                            eew.EarthQuake.Location.Long = double.NaN;
                            eew.EarthQuake.Depth = -1;
                        }
                    }
                    //海域かどうか
                    if (string.Equals(root.XPathSelectElement($"/jmx:Report/{GetName("Body")}/{GetName("Earthquake")}/{GetName("Hypocenter")}/{GetName("Area")}/{GetName("LandOrSea")}", nsManager)?.Value,"海域"))
                        eew.EarthQuake.IsSea = true;
                    //最大震度の取得
                    var intensity = Common.StringToInt(root.XPathSelectElement($"/jmx:Report/{GetName("Body")}/{GetName("Intensity")}/{GetName("Forecast")}/{GetName("ForecastInt")}/*[2]", nsManager)?.Value);
                    //Log.Instance.Debug($"/jmx:Report/{GetName("Body")}/{GetName("Intensity")}/{GetName("Forecast")}/{GetName("ForecastInt")}/*[1]");
                    if (intensity == Common.Intensity.Unknown) intensity = Common.StringToInt(root.XPathSelectElement($"/jmx:Report/{GetName("Body")}/{GetName("Intensity")}/{GetName("Forecast")}/{GetName("ForecastInt")}/*[1]", nsManager)?.Value);
                    eew.EarthQuake.MaxIntensity = intensity;
                    for (int i = 0; i < 1000; i++)
                    {
                        tmp = root.XPathSelectElement($"/jmx:Report/*[\"Body\"]/*[\"Intensity\"]/*[\"Forecast\"]/*[local-name()=\"Pref\"][{i + 1}]/*[\"Area\"]/*", nsManager)?.Value;
                        if (string.IsNullOrEmpty(tmp)) break;
                        Struct.cEEW.AreaInfo areaInfo = new()
                        {
                            Prefectures = Common.StringToPrefectures(root.XPathSelectElement($"/jmx:Report/*[\"Body\"]/*[\"Intensity\"]/*[\"Forecast\"]/*[local-name()=\"Pref\"][{i + 1}]/*[local-name()=\"Name\"]", nsManager)?.Value ?? ""),
                            Name = tmp,
                            Intensity = Common.StringToInt(root.XPathSelectElement($"/jmx:Report/*[\"Body\"]/*[\"Intensity\"]/*[\"Forecast\"]/*[local-name()=\"Pref\"][{i + 1}]/*[local-name()=\"Area\"]/*[local-name()=\"ForecastInt\"]/*[\"To\"]", nsManager)?.Value),
                        };
                        tmp = root.XPathSelectElement($"/jmx:Report/*[\"Body\"]/*[\"Intensity\"]/*[\"Forecast\"]/*[local-name()=\"Pref\"][{i + 1}]/*[local-name()=\"Name\"]", nsManager)?.Value;
                        if (!string.IsNullOrEmpty(tmp) && tmp.Contains("北海道")) tmp = "北海道";
                        if (!string.IsNullOrEmpty(tmp)) areaInfo.Prefectures = Common.StringToPrefectures(tmp);
                        //Log.Instance.Debug(root.XPathSelectElement($"/jmx:Report/*[\"Body\"]/*[\"Intensity\"]/*[\"Forecast\"]/*[local-name()=\"Pref\"][{i + 1}]/*[local-name()=\"Name\"]", nsManager)?.Value??"");
                        if (!DateTime.TryParse(root.XPathSelectElement($"/jmx:Report/*[\"Body\"]/*[\"Intensity\"]/*[\"Forecast\"]/*[local-name()=\"Pref\"][{i + 1}]/*[local-name()=\"Area\"]/*[local-name()=\"ArrivalTime\"]", nsManager)?.Value, out areaInfo.ExpectedArrival)) areaInfo.ExpectedArrival = DateTime.MinValue;
                        var text = root.XPathSelectElement($"/jmx:Report/*[\"Body\"]/*[\"Intensity\"]/*[\"Forecast\"]/*[local-name()=\"Pref\"][{i + 1}]/*[local-name()=\"Area\"]/*[\"Category\"]/*[\"Kind\"]/*[\"Name\"]", nsManager)?.Value;
                        if(string.Equals(text,"緊急地震速報（警報）"))
                        {
                            eew.Serial.Infomation = Struct.EEW.InfomationLevel.Warning;
                        }
                        

                        if(areaInfo.Intensity>=Struct.Common.Intensity.Int5Down) eew.EarthQuake.ForecastArea.Regions.Add(Struct.EEWArea.StrToRegion(areaInfo.Name));
                        eew.AreasInfo.Add(areaInfo);
                    }
                    for(int i = 0; i < 100; i++)
                    {
                        var t= root.XPathSelectElement($"/jmx:Report/{GetName("Head")}/{GetName("Headline")}/{GetName("Information")}[@type='緊急地震速報（地方予報区）']/{GetName("Item")}/{GetName("Areas")}/{GetName("Area",i)}", nsManager);
                        if (t == null) break;
                        string pos = t.XPathSelectElement($"{GetName("Code")}")?.Value??"";
                        if (int.TryParse(pos, out var id))
                            eew.EarthQuake.ForecastArea.District.Add((EEWArea.District)id);
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        var t = root.XPathSelectElement($"/jmx:Report/{GetName("Head")}/{GetName("Headline")}/{GetName("Information")}[@type='緊急地震速報（府県予報区）']/{GetName("Item")}/{GetName("Areas")}/{GetName("Area", i)}", nsManager);
                        if (t == null) break;
                        string pos = t.XPathSelectElement($"{GetName("Code")}")?.Value ?? "";
                        if (int.TryParse(pos, out var id))
                            eew.EarthQuake.ForecastArea.LocalAreas.Add((EEWArea.LocalAreas)id);
                    }
                    for (int i = 0; i < 1000; i++)
                    {
                        var t = root.XPathSelectElement($"/jmx:Report/{GetName("Head")}/{GetName("Headline")}/{GetName("Information")}[@type='緊急地震速報（細分区域）']/{GetName("Item")}/{GetName("Areas")}/{GetName("Area", i)}", nsManager);
                        if (t == null) break;
                        string pos = t.XPathSelectElement($"{GetName("Code")}")?.Value ?? "";
                        if (int.TryParse(pos, out var id))
                            eew.EarthQuake.ForecastArea.Regions.Add((EEWArea.Regions)id);
                    }
                    /*foreach ( var area in eew.EarthQuake.ForecastArea.Regions)
                    {
                        var n = Struct.EEWArea.RegionToLocal(area);
                        if (eew.EarthQuake.ForecastArea.LocalAreas.Find(a => a == n) == default)
                            eew.EarthQuake.ForecastArea.LocalAreas.Add(n);
                    }
                    var district = Struct.EEWArea.LocalToDistrict(eew.EarthQuake.ForecastArea.LocalAreas);
                    eew.EarthQuake.ForecastArea.District.AddRange(district);*/

                    var n = "";
                    foreach (var area in eew.EarthQuake.ForecastArea.District) n += $"{EEWArea.DistrictToStr(area)} ";
                    n += "\n";
                    foreach (var area in eew.EarthQuake.ForecastArea.LocalAreas) n += $"{EEWArea.LocalAreasToStr(area)} ";
                    n += "\n";
                    foreach (var area in eew.EarthQuake.ForecastArea.Regions) n += $"{EEWArea.RegionsToStr(area)} ";
                    Log.Debug($"地震識別ID : {eew.Serial.EventID}\n" +
                        $"情報番号 : {eew.Serial.Number}\n" +
                        $"最終報 : {(eew.Serial.IsFinal ? "はい" : "いいえ")}\n" +
                        $"緊急地震速報の種類 : {eew.Serial.Infomation}\n" +
                        $"発表時刻 : {eew.Serial.UpdateTime}\n" +
                        $"発生時刻 : {eew.EarthQuake.OriginTime}\n" +
                        $"地震の規模 : {eew.EarthQuake.Magnitude}\n" +
                        $"震源地 : {eew.EarthQuake.Hypocenter}({eew.EarthQuake.HypocenterCode})\n" +
                        $"緯度経度 : E{eew.EarthQuake.Location.Long} N{eew.EarthQuake.Location.Lat}\n" +
                        $"震源の深さ : {eew.EarthQuake.Depth} km\n" +
                        $"海域 : {(eew.EarthQuake.IsSea ? "はい" : "いいえ")}\n" +
                        $"最大震度 : {Struct.Common.IntToStringLong(eew.EarthQuake.MaxIntensity)}\n" +
                        $"地域ポイント数 : {eew.AreasInfo.Count}\n" +
                        $"警報地域数 : {eew.EarthQuake.ForecastArea.Regions.Count} / {eew.EarthQuake.ForecastArea.LocalAreas.Count} / {eew.EarthQuake.ForecastArea.District.Count}\n" +
                        /*$""); //*/ $"{n}");



                    TempData = eew;
                    //イベントの発生
                    if (UpdateHandler != null)
                    {
                        var args = new EEWEventArgs(null, eew);
                        UpdateHandler(null, args);
                    }
                    else Log.Error("UpdateHandlerがnullです");
                }
            } catch (Exception ex)
            {
                Log.Error(ex);
            }

        }
        static string GetName(string name, int? array = null)
        {
            if (array != null) return $"*[local-name()=\"{name}\"][{array + 1}]";
            return $"*[local-name()=\"{name}\"]";
        }
        public Struct.EEW GetEEW()
        {
            Log.Debug("過去の緊急地震速報のデータを取得しました。");
            return TempData;
        }
        
        private async void SocketConnection()
        {
            while (true)
            {
                try
                {
                    if (Client == null) throw new ArgumentNullException(nameof(Client), "APIを初期化してください。");
                    Log.Debug("ソケットは接続中です。");
                    EndTask = false;
                    Socket = new DmdataV2Socket(Client);
                    Socket.Connected += SocketConnected;
                    Socket.Disconnected += SocketDisconnected;
                    Socket.Error += SocketError;
                    Socket.DataReceived += SocketReceive;
                    if (IsWarnOnly)
                    {
                        await Socket.ConnectAsync(new DmdataSharp.ApiParameters.V2.SocketStartRequestParameter(
                            TelegramCategoryV1.EewWarning
            )
                        {
                            AppName = $"{Properties.Version.Name}",
                        });
                    }
                    else
                    {
                        await Socket.ConnectAsync(new DmdataSharp.ApiParameters.V2.SocketStartRequestParameter(
                            TelegramCategoryV1.EewForecast
            )
                        {
                            AppName = $"{Properties.Version.Name}",
                        });
                    }
                    break;
                }catch(Exception ex)
                {
                    Log.Error(ex);
                    await Task.Delay(3000);
                }
            }
        }
    }
}
