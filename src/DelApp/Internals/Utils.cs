using DelApp.Internals.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DelApp.Internals
{
    internal static class Utils
    {
        private const int TOKEN_ADJUST_PRIVILEGES = 0x20;

        public static readonly string MyGuidString = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();
        public static readonly char[] MyGuidStringWithNullChar = ToCharArrayWithNullChar(MyGuidString);

        public static readonly IntPtr AppProcessHandle = NativeMethods.GetCurrentProcess();
        public static readonly int AppProcessId = NativeMethods.GetProcessId(AppProcessHandle);

        public static bool IsWow64Process(IntPtr processHandle) => NativeMethods.IsWow64Process(processHandle, out bool isWow64) && isWow64;

        public static void EnablePrivileges()
        {
            if (NativeMethods.OpenProcessToken(AppProcessHandle, TOKEN_ADJUST_PRIVILEGES, out IntPtr hToken))
            {
                var mtkp = new SingleTokenPrivilegeOn();
                AddPrivileges(hToken, mtkp, "SeDebugPrivilege");
                AddPrivileges(hToken, mtkp, "SeBackupPrivilege");
                AddPrivileges(hToken, mtkp, "SeRestorePrivilege");
                NativeMethods.CloseHandle(hToken);
            }
            void AddPrivileges(IntPtr token, SingleTokenPrivilegeOn tkp, string pn)
            {
                if (NativeMethods.LookupPrivilegeValueW(null, pn, out LUID luid))
                {
                    tkp.Luid = luid;
                    NativeMethods.AdjustTokenPrivileges(token, false, tkp, 0, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        public static string GetPathByFileHandle(IntPtr fileHandle)
        {
            char[] buffer = ObjPool.RentCharBuffer();
            try
            {
                int len = NativeMethods.GetFinalPathNameByHandleW(fileHandle, buffer, buffer.Length - 1, 0);
                if (len == 0)
                    return null;
                return new string(buffer, 4, len - 4);
            }
            finally
            {
                ObjPool.ReturnCharBuffer(buffer);
            }
        }




        public static IEnumerable<FileNDir> GetDrapDropFiles(IntPtr hDrop)
        {

            int fileCount = GetDragFileCount(hDrop);
            if (fileCount <= 0)
                goto Final_Drag_Final;
            InternelDriveInfo.RefreshDriveCache();
            for (uint i = 0; i < fileCount; i++)
            {
                var file = CreateFromDropItem(hDrop, i);
                if (!file.IsDrive && file.Exists)
                    yield return CreateFromDropItem(hDrop, i);
            }
        Final_Drag_Final:
            NativeMethods.DragFinish(hDrop);

            int GetDragFileCount(IntPtr hd)
            {
                unsafe
                {
                    return NativeMethods.DragQueryFileW(hd, uint.MaxValue, null, 0);
                }
            }

            FileNDir CreateFromDropItem(IntPtr hd, uint index)
            {
                char[] pathBuf = ObjPool.RentCharBuffer();
                unsafe
                {
                    fixed (char* p = pathBuf)
                    {
                        int len = NativeMethods.DragQueryFileW(hd, index, p, pathBuf.Length);
                        try
                        {
                            return new FileNDir(new string(p, 0, len));
                        }
                        finally
                        {
                            ObjPool.ReturnCharBuffer(pathBuf);
                        }
                    }
                }
            }
        }


        public static void WriteErrorLog(string info)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = File.Open("DelAppError.log", FileMode.Append);
                sw = new StreamWriter(fs);
                sw.Write(info);
            }
            catch { }
            finally
            {
                sw?.Dispose();
                fs?.Dispose();
            }
        }







        private static char[] ToCharArrayWithNullChar(string str)
        {
            int length = str.Length;
            var chaArray = new char[length + 1];
            str.CopyTo(0, chaArray, 0, length);
            return chaArray;
        }

    }
}
