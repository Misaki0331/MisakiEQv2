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
            // UnhandledException�C�x���g�E�n���h����o�^����
            Thread.GetDomain().UnhandledException += new
            UnhandledExceptionEventHandler(Application_UnhandledException);
#endif
#if ADMIN
            string mutexName = "MisakiEQ(Admin Mode)";
#elif !DEBUG
            string mutexName = "MisakiEQ";
#endif

#if !DEBUG
            //Mutex�I�u�W�F�N�g���쐬����
            mutex = new(false, mutexName);
            try
            {
                //�~���[�e�b�N�X�̏��L����v������
                hasHandle = mutex.WaitOne(0, false);
            }
            //.NET Framework 2.0�ȍ~�̏ꍇ
            catch (System.Threading.AbandonedMutexException)
            {
                //�ʂ̃A�v���P�[�V�������~���[�e�b�N�X��������Ȃ��ŏI��������
                hasHandle = true;
            }
            //�~���[�e�b�N�X�𓾂�ꂽ�����ׂ�
            if (hasHandle == false)
            {
                //�����Ȃ������ꍇ�́A���łɋN�����Ă���Ɣ��f���ďI��
                MessageBox.Show("MisakiEQ�͑��d�N���ł��܂���B\n�����\������Ȃ��ꍇ�̓^�X�N�g���C���`�F�b�N���Ă��������B", "MisakiEQ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Log.Logger.GetInstance().Fatal("�X���b�h�ɂ���O�G���[������");
            Log.Logger.GetInstance().Fatal(e.Exception);
            ShowErrorMessage(e.Exception, "�X���b�h�ɂ���O�G���[");
        }

        // ��������O���L���b�`����C�x���g�E�n���h��
        // �i��ɃR���\�[���E�A�v���P�[�V�����p�j
        public static void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Log.Logger.GetInstance().Fatal("�z��O�̃G���[������");
                Log.Logger.GetInstance().Fatal(ex);
                ShowErrorMessage(ex, "�n���h������Ă��Ȃ���O�G���[");
            }
        }

        // ���[�U�[�E�t�����h���ȃ_�C�A���O��\�����郁�\�b�h
        public static void ShowErrorMessage(Exception ex, string extraMessage)
        {
            string ErrorString = "";
            ErrorString += extraMessage + " \n";
            ErrorString += "�y��O�G���[���z\n" + ex.GetType().ToString() + "\n\n";
            ErrorString += "�y��O�������������]�b�g�z\n" + ex.TargetSite + "\n\n";
            ErrorString += "�y��O�����������\�[�X�z\n" + ex.Source + "\n\n";
            ErrorString += "�y�G���[���e�z\n" + ex.Message + "\n\n";
            ErrorString += "�y�X�^�b�N�g���[�X�z\n" + ex.StackTrace;
            GUI.ErrorInfo.UnhandledException err = new(ErrorString, ErrorCount,$"{ex.TargetSite}");
#if !DEBUG
            TrayHub.DisposeInstance();
            if (hasHandle)
            {
                //�~���[�e�b�N�X���������
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