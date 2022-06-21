using MisakiEQ.GUI;

namespace MisakiEQ
{
    internal static class Program
    {
        private static int ErrorCount = 0;

#if !DEBUG
        static Mutex? mutex=null;
        static bool hasHandle = false;
#endif
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
#if !DEBUG
            Application.ThreadException += new
         ThreadExceptionEventHandler(Application_ThreadException);
            // UnhandledExceptionイベント・ハンドラを登録する
            Thread.GetDomain().UnhandledException += new
            UnhandledExceptionEventHandler(Application_UnhandledException);
#endif
#if ADMIN
            string mutexName = "MisakiEQ(Admin Mode)";
#elif !DEBUG
            string mutexName = "MisakiEQ";
#endif

#if !DEBUG
            //Mutexオブジェクトを作成する
            mutex = new(false, mutexName);
            try
            {
                //ミューテックスの所有権を要求する
                hasHandle = mutex.WaitOne(0, false);
            }
            //.NET Framework 2.0以降の場合
            catch (System.Threading.AbandonedMutexException)
            {
                //別のアプリケーションがミューテックスを解放しないで終了した時
                hasHandle = true;
            }
            //ミューテックスを得られたか調べる
            if (hasHandle == false)
            {
                //得られなかった場合は、すでに起動していると判断して終了
                MessageBox.Show("MisakiEQは多重起動できません。\nもし表示されない場合はタスクトレイをチェックしてください。", "MisakiEQ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            for (int i = 0; args.Length > i; i++)
                if (args[i].StartsWith("ErrorFlg="))
                    ErrorCount = int.Parse(args[i].Remove(0, 9));
            ApplicationConfiguration.Initialize();
            TrayHub.GetInstance();
            Application.Run();
#if !DEBUG
            if (hasHandle)
                mutex.ReleaseMutex();
#endif
            Log.Logger.GetInstance().Info("Application was successfully exited.");
        }
        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.Logger.GetInstance().Fatal("スレッドによる例外エラーが発生");
            Log.Logger.GetInstance().Fatal(e.Exception);
            ShowErrorMessage(e.Exception, "スレッドによる例外エラー");
        }

        // 未処理例外をキャッチするイベント・ハンドラ
        // （主にコンソール・アプリケーション用）
        public static void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Log.Logger.GetInstance().Fatal("想定外のエラーが発生");
                Log.Logger.GetInstance().Fatal(ex);
                ShowErrorMessage(ex, "ハンドルされていない例外エラー");
            }
        }

        // ユーザー・フレンドリなダイアログを表示するメソッド
        public static void ShowErrorMessage(Exception ex, string extraMessage)
        {
            string ErrorString = "";
            ErrorString += extraMessage + " \n";
            ErrorString += "【例外エラー名】\n" + ex.GetType().ToString() + "\n\n";
            ErrorString += "【例外が発生したメゾット】\n" + ex.TargetSite + "\n\n";
            ErrorString += "【例外が発生したソース】\n" + ex.Source + "\n\n";
            ErrorString += "【エラー内容】\n" + ex.Message + "\n\n";
            ErrorString += "【スタックトレース】\n" + ex.StackTrace;
            GUI.ErrorInfo.UnhandledException err = new(ErrorString, ErrorCount,$"{ex.TargetSite}");
#if !DEBUG
            TrayHub.DisposeInstance();
            if (hasHandle)
            {
                //ミューテックスを解放する
                if (mutex != null)
                {
                    mutex.ReleaseMutex();
                    mutex = null;
                }
            }
#endif
            err.Show();
        }
    }

}