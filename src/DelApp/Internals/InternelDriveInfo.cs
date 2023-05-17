using DelApp.Internals.Win32;
using System;
using System.Collections.Generic;
using System.IO;

namespace DelApp.Internals
{
    internal static class InternelDriveInfo
    {
        private static readonly int s_pro_drive_count = Environment.GetLogicalDrives().Length << 1;
        private static readonly Dictionary<string, string> s_driveCache = new Dictionary<string, string>(s_pro_drive_count);
        private static readonly Dictionary<string, string> s_driveDosNameMap = new Dictionary<string, string>(s_pro_drive_count);

        public static bool IsDrive(string path) => s_driveCache.ContainsKey(path);

        public static void RefreshDriveCache()
        {
            s_driveCache.Clear();
            s_driveDosNameMap.Clear();
            foreach (string item in RefreshDriveCore())
            {
                AddToCacheCore(item);
            }
        }


        public static IEnumerable<string> RefreshDriveCacheAndReturnsDriveName()
        {

            s_driveCache.Clear();
            s_driveDosNameMap.Clear();

            foreach (string item in RefreshDriveCore())
            {
                AddToCacheCore(item);
                yield return item;
            }
        }


        public static string GetNtPathFromDosPath(IntPtr dosPath, int charCount)
        {
            unsafe
            {
                var s = (char*)dosPath.ToPointer();
                return GetNtPathFromDosPathCore(s, charCount);
            }

        }

        public static string GetNtPathFromDosPath(char[] dosPath, int charCount)
        {
            unsafe
            {
                fixed (char* s = dosPath)
                    return GetNtPathFromDosPathCore(s, charCount);
            }
        }


        private static unsafe string GetNtPathFromDosPathCore(char* s, int charCount)
        {
            int length;
            int i = 0, j = 0;
            foreach (KeyValuePair<string, string> item in s_driveDosNameMap)
            {
                length = item.Key.Length;
                if (length < charCount)
                {
                    fixed (char* pDos = item.Key)
                    {
                        while (i < length)
                        {
                            if (pDos[i] != s[i])
                            {
                                goto final;
                            }
                            ++i;
                        }
                        i -= item.Value.Length;
                        fixed (char* pWin = item.Value)
                        {
                            while (i < length)
                            {
                                s[i++] = pWin[j++];
                            }
                        }
                        length = item.Key.Length - item.Value.Length;
                        return new string(s, length, charCount - length);

                    }
                }

            final:
                i = 0;
                j = 0;
            }
            return null;
        }



        private static void AddToCacheCore(string driveName)
        {
            s_driveCache.Add(driveName, null);
            string deviceName = driveName.Substring(0, driveName.Length - 1);
            var result = TryQueryDosDevice(deviceName);
            if (result != null)
            {
                s_driveDosNameMap.Add(result, deviceName);
            }

            string TryQueryDosDevice(string ntDeviceName)
            {
                char[] pathBuf = ObjPool.RentCharBuffer();
                unsafe
                {
                    fixed (char* p = pathBuf)
                    {
                        int len = NativeMethods.QueryDosDeviceW(ntDeviceName, p, pathBuf.Length);
                        try
                        {
                            return len == 0 ? null : new string(p, 0, len - 2);
                        }
                        finally
                        {
                            ObjPool.ReturnCharBuffer(pathBuf);
                        }
                    }
                }

            }
        }

        private static IEnumerable<string> RefreshDriveCore()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            int length = drives.Length;
            DriveInfo di;
            DriveType dt;
            for (int i = 0; i < length; i++)
            {
                di = drives[i];
                dt = di.DriveType;
                if (di.IsReady && (dt == DriveType.Fixed || dt == DriveType.Removable))
                {
                    yield return di.Name;
                }
            }
        }


    }
}
