using DelApp.Internals;
using DelApp.Locals;
using System;
using System.Threading;
using System.Windows.Forms;

namespace DelApp
{
    internal static class Program
    {

        public static string FilePath;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            InternelDriveInfo.RefreshDriveCache();
            string[] args = Environment.GetCommandLineArgs();
            string fullPath = args.Length > 1 ? FileNDir.GetFullPath(args[1]) : null;

            using (var mutex = new Mutex(true, Utils.MyGuidString, out bool createdNew))
            {
                if (createdNew)
                {
                    FilePath = fullPath;
                    if (PipeService.CreateService())
                        StartApp();
                }
                else if (fullPath != null)
                {
                    PipeService.SendPath(fullPath);
                }
            }
        }


        static void StartApp()
        {
            // Todo : add new language providers here.
            AppLanguageProviderChs.Instance.Register();

            Utils.EnablePrivileges();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
            RestartManagerHelper.ReleaseSharedInstance();
            SoundPlayHelper.ReleaseSharedInstance();
        }



        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) => Utils.WriteErrorLog(e.ExceptionObject.ToString());




    }
}
