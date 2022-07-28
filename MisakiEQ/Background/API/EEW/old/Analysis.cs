using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Background.API.EEW.OLD
{
    public class Analysis
    {
        EEW.OLD.JSON.Root? Data = new();
        public async         Task
Loop(Config config, EventHandler<EEWEventArgs>? UpdateHandler, CancellationToken token)
        {
            bool IsTest = false;
            bool IsFirst = true;
            Stopwatch TSW = new();
            TSW.Start();
            long TempDelay = 0;
            long TempDetect = long.MinValue;
            string OldTemp = "";
            while (true)
            {
                string json = "";
                try
                {
                    if (TSW.ElapsedMilliseconds >= TempDelay)
                    {

                        var task = Lib.WebAPI.GetString("https://api.iedred7584.com/eew/json/", token);
                        try
                        {
                            await task;
                        }
                        catch (Exception ex)
                        {
                            Log.Instance.Warn($"取得時にエラーが発生しました。{ex.Message}");
                        }
                        if (task.IsCompletedSuccessfully && !string.IsNullOrEmpty(task.Result))
                        {
                            json = task.Result;
                            if (IsTest) json = Properties.Resources.testForecast;
                            if (OldTemp != json)
                            {
                                TempDetect = TSW.ElapsedMilliseconds;
                                OldTemp = json;
                                Data = JsonConvert.DeserializeObject<EEW.OLD.JSON.Root>(json);
                                if (Data != null && Data.ParseStatus == "Success")
                                {
                                    if (IsFirst)
                                    {
                                        IsFirst = false;
                                    }
                                    else
                                    {
                                        if (UpdateHandler != null)
                                        {
                                            var args = new EEWEventArgs(Data, GetData());
                                            UpdateHandler(null, args);
                                        }
                                    }
                                }
                            }
                        }

                        long count = 0;
                        if (TempDetect > TSW.ElapsedMilliseconds - config.DelayDetectCoolDown)
                        {
                            count = TSW.ElapsedMilliseconds / config.DelayDetectMode;
                            TempDelay = config.DelayDetectMode * (count + 1);
                        }
                        else
                        {
                            count = TSW.ElapsedMilliseconds / config.Delay;
                            TempDelay = config.Delay * (count + 1);
                        }
                    }
                    await Task.Delay(10, token);
                }
                catch (TaskCanceledException ex)
                {
                    Log.Instance.Info($"スレッドの処理を終了します。{ex.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Log.Instance.Error($"文字列データ : \"{json}\"");
                    Log.Instance.Error(ex);
                }
            }
        }
        public Struct.EEW GetData(Struct.EEW? from = null)
        {
            if (from == null) from = new();
            if (Data != null)
            {
                Log.Instance.Debug("汎用クラスに変換中...");
                from = Struct.EEW.GetData(Data, from);
                Log.Instance.Debug("汎用クラスに変換完了");
            }
            else
            {
                Log.Instance.Warn("APIの情報がありませんでした。");
            }
            return from;

        }
    }

}
