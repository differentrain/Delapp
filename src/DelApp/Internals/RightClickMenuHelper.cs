using DelApp.Locals;
using Microsoft.Win32;
using System.Windows.Forms;

namespace DelApp.Internals
{
    internal static class RightClickMenuHelper
    {

        private static readonly string s_dir_subkey = $"Directory\\shell\\{Utils.MyGuidString}\\command";
        private static readonly string s_file_subkey = $"*\\shell\\{Utils.MyGuidString}\\command";
        private static readonly string s_open_command = $"{Application.ExecutablePath} \"%L\"";

        public static bool HasRightClickMenu
        {
            get => GetHasRightMenu();
            set
            {
                if (value)
                    AddRightClickMenu();
                else
                    RemoveRightClickMenu();
            }
        }

        private static bool GetHasRightMenu()
        {
            using (RegistryKey regRoot0 = Registry.ClassesRoot.OpenSubKey("Directory\\shell", true),
                regRoot1 = Registry.ClassesRoot.OpenSubKey("*\\shell", true))
            {
                if (!GetHasRrghtMenuCore(regRoot0))
                    return false;
                else if (!GetHasRrghtMenuCore(regRoot1))
                {
                    RemoveRightClickMenuCore(regRoot0);
                    return false;
                }
                else
                    return true;
            }
        }


        private static bool GetHasRrghtMenuCore(RegistryKey regRoot)
        {
            using (RegistryKey regshell = regRoot?.OpenSubKey(Utils.MyGuidString, true))
            {
                if (regshell == null)
                    return false;
                regshell.SetValue(null, AppLanguageService.LanguageProvider.Shell_RightClickContextText);
                regshell.SetValue("icon", Application.ExecutablePath);
                using (RegistryKey regCmd = regshell.CreateSubKey("command"))
                {
                    if (regCmd == null)
                    {
                        regRoot.DeleteSubKeyTree(Utils.MyGuidString);
                        return false;
                    }
                    regCmd.SetValue(null, s_open_command);
                }
            }
            return true;
        }

        // Do not need set value, since we always invokes HasRightClickMenu_get .
        private static void AddRightClickMenu()
        {
            using (RegistryKey regRoot = Registry.ClassesRoot.CreateSubKey(s_dir_subkey, true))
            {
            }
            using (RegistryKey regRoot = Registry.ClassesRoot.CreateSubKey(s_file_subkey, true))
            {
            }
        }

        private static void RemoveRightClickMenu()
        {
            using (RegistryKey regRoot0 = Registry.ClassesRoot.OpenSubKey("Directory\\shell", true),
                               regRoot1 = Registry.ClassesRoot.OpenSubKey("*\\shell", true))
            {
                RemoveRightClickMenuCore(regRoot0);
                RemoveRightClickMenuCore(regRoot1);
            }
        }

        private static void RemoveRightClickMenuCore(RegistryKey regRoot)
        {
            using (RegistryKey regshell = regRoot?.OpenSubKey(Utils.MyGuidString, true))
            {
                if (regshell != null)
                    regRoot.DeleteSubKeyTree(Utils.MyGuidString);
            }
        }
    }
}
