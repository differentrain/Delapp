using DelApp.Internals;
using DelApp.Locals;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace DelApp
{
    internal static class Program
    {

        public static IEnumerable<string> FilePathes;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;




            using (var mutex = new Mutex(true, Utils.MyGuidString, out bool createdNew))
            {
                if (createdNew)
                {
                    FilePathes = GetPathes();
                    if (PipeService.CreateService())
                        StartApp();
                }
                else
                {
                    PipeService.SendPath(GetPathes());
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

        static IEnumerable<string> GetPathes()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
                yield break;
            string path;
            InternelDriveInfo.RefreshDriveCache();
            for (int i = 1; i < args.Length; i++)
            {
                path = FileNDir.GetFullPath(args[i]);
                if (path != null)
                    yield return path;
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) => Utils.WriteErrorLog(e.ExceptionObject.ToString());




    }
}
