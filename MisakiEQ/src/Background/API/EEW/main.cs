using MisakiEQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MisakiEQ.Background.API
{
#pragma warning disable IDE1006 // 命名スタイル
    public class _EEW
#pragma warning restore IDE1006 // 命名スタイル
    {
        private readonly Stopwatch TSW = new();//thread起動状態兼タイマー
        public EEW.OLD.Analysis OldAPI = new();//古いAPI(削除する予定)
        public EEW.dmdata.Analysis DMData = new();
        public enum APIServer
        {
            OldAPI,
            K_Moni,
            Dmdata
        }
        public APIServer CurrentAPI = APIServer.Dmdata;
        public EEW.Config Config = new();
        public EEW.OLD.JSON.Root? Data = new();
        public event EventHandler<EEWEventArgs>? UpdateHandler;

        Task? Threads;
        static readonly CancellationTokenSource CancelTokenSource = new();
        static readonly CancellationToken CancelToken = CancelTokenSource.Token;
        public _EEW()
        {
            Config.Delay = 1000;
            Config.DelayDetectMode = 200;
            Config.DelayDetectCoolDown = 3000;
        }
        public void Init()
        { }
        public void RunThread()
        {
            if (Threads == null || Threads.Status != TaskStatus.Running)
            {
                Log.Debug("スレッド開始の準備を開始します。");
                TSW.Restart();
                Threads = Task.Run(() => ThreadFunction(CancelToken));
            }
            else
            {
                Log.Error("該当スレッドは動作中の為、起動ができませんでした。");
            }

        }
        /*public void AbortThread()
        {
            Log.Instance.Debug("スレッド破棄の準備を開始します。");
            CancelTokenSource.Cancel();
        }*/
        public async Task AbortAndWait()
        {
            Log.Debug("スレッドを終了しています...");
            CancelTokenSource.Cancel();
            if (Threads != null && !Threads.IsCompleted) await Threads;
        }
        public bool GetThreadWorking()
        {
            if (Threads == null) return false;
            return Threads.Status == TaskStatus.Running;
        }
        public long GetThreadTimer() //100ナノ秒単位
        {
            return TSW.ElapsedTicks;
        }

        public bool IsTest = false;

        private async Task ThreadFunction(CancellationToken token)
        {
            Log.Info("スレッド開始");
            var looping = true;
            while (looping) 
            {
                looping = false;
                switch (CurrentAPI)
                {
                    case APIServer.OldAPI:
                        await OldAPI.Loop(Config, UpdateHandler, token);
                        break;
                    case APIServer.Dmdata:
                        if (!DMData.Init())
                        {
                            Log.Warn("トークンがセットできない為他のAPIを使用します。");
                            CurrentAPI=APIServer.OldAPI;
                            looping = true;
                            continue;
                        }
                        DMData.UpdateHandler += GetEvent;
                        await DMData.Loop(token);
                        DMData.UpdateHandler -= GetEvent;
                        DMData.APIClose();
                        break;
                    default:
                        Log.Error("目的のAPIが存在しません");
                        break;
                }
            }
            Log.Info("スレッド終了");
        }
        public void GetEvent(object? sender, EEWEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                //イベントの発生
                if (UpdateHandler != null)
                {
                    var args = new EEWEventArgs(null, e.eew);
                    UpdateHandler(null, args);
                    break;
                }
                else
                {
                    Log.Info("地震イベントを再連携します。");
                    try
                    {
                        GUI.TrayHub.GetInstance()?.ResetEventEEW();
                    }catch(Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
            }
        }
        public Struct.EEW GetData(Struct.EEW? from=null)
        {
            switch (CurrentAPI)
            {
                case APIServer.OldAPI:
                    Log.Debug("OldAPI");
                    return OldAPI.GetData(from);
                case APIServer.Dmdata:
                    Log.Debug("DMDATA");
                    return DMData.GetEEW();
                default:
                    Log.Error("目的のAPIが存在しません");
                    from = new();
                    return from;
            }
        }
    }

    public class EEWEventArgs
    {
        public EEW.OLD.JSON.Root? json = null;
        public Struct.EEW? eew = null;

        public EEWEventArgs(EEW.OLD.JSON.Root? root , Struct.EEW? eew)
        {
            this.json = root;
            this.eew = eew;
        }
    }
}
