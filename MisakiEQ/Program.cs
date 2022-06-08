using MisakiEQ.GUI;

namespace MisakiEQ
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            TrayHub.GetInstance();
            Application.Run();
            Log.Logger.GetInstance().Info("Application was successfully exited.");
        }
    }
}