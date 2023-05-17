using DelApp.Internals.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DelApp.Internals
{
    internal static class FileUnlocker
    {
        private const int FILE_TYPE_DISK = 1;
        private const int DuplicateCloseSource = 0x00000001;

        private const int SystemHandleInformation = 0x0010;
        private const uint STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;
        private const uint Success = 0x00000000;

        private const int ALL_ACCESS = 0x001F0FFF;

        private const int LIST_MODULES_ALL = 0x03;

        private const int MemoryBasicInformation = 0;
        private const int MemorySectionName = 2;
        private const int MemInfoBufferSizeInt = 32767 * 2;
        private const int MEM_Commit = 0x1000;
        private const int MEM_Mapped = 0x40000;

        private const int DuplicateSameAccess = 0x00000002;

        private static readonly IntPtr s_stemMinAppAddress;
        private static readonly IntPtr s_systemMaxAppAddress;
        private static readonly IntPtr s_systemMaxApp32Address;

        private static readonly IntPtr s_memInfoBufferSizeIntPtr = new IntPtr(MemInfoBufferSizeInt);

        private static readonly int s_handleEntrySize = 8 + IntPtr.Size * 2;
        private static readonly int s_handleInfoSize = s_handleEntrySize + IntPtr.Size;

        static FileUnlocker()
        {
            var systemInfo = new SysInfo();
            NativeMethods.GetSystemInfo(systemInfo);
            long maxAddr = systemInfo.MaximumApplicationAddress.ToInt64();
            s_systemMaxApp32Address = new IntPtr(maxAddr & 0x7FFFFFFF);
            s_systemMaxAppAddress = (ulong)maxAddr <= uint.MaxValue ? s_systemMaxApp32Address : systemInfo.MaximumApplicationAddress;
            s_stemMinAppAddress = systemInfo.MinimumApplicationAddress;
        }


        public static void UnlockModuleAndMemory(Dictionary<string, int> pathes)
        {
            using (var buffer = new UnmanagedBuffer(1024 * 1024))
            {
                bool success;
                int realSize;
                while ((success = NativeMethods.K32EnumProcesses(buffer.Handle, buffer.Size, out realSize)) && buffer.Size <= realSize)
                {
                    buffer.Resize(realSize << 1);
                }
                if (!success)
                    return;
                unsafe
                {
                    ReleaseModuleAndMemory(pathes, (int*)buffer.Handle.ToPointer(), realSize >> 2);
                }
            }
        }

        public static void UnlockHandle(Dictionary<string, int> pathes)
        {

            ReleaseHandle(null, pathes);
        }

        public static void UnlockModuleAndMemory(Dictionary<string, int> pathes, int[] locker)
        {
            unsafe
            {
                fixed (int* p = locker)
                    ReleaseModuleAndMemory(pathes, p, locker.Length);
            }
        }

        public static void UnlockHandle(Dictionary<string, int> pathes, int[] locker)
        {
            Dictionary<int, int> handleLocker = locker.Where(p => p >= 0).ToDictionary(p => p, p => p);
            ReleaseHandle(handleLocker, pathes);
        }



        private unsafe static void ReleaseModuleAndMemory(Dictionary<string, int> pathes, int* locker, int length)
        {
            Parallel.For(0, length, i =>
            {
                IntPtr handle = NativeMethods.OpenProcess(ALL_ACCESS, false, locker[i]);
                if (handle == IntPtr.Zero || ReleaseModules(handle, pathes) || ReleaseMappedFile(handle, pathes))
                    locker[i] = -1;
                NativeMethods.CloseHandle(handle);

            });
        }

        private static bool ReleaseModules(IntPtr handle, Dictionary<string, int> pathes)
        {
            using (var buffer = new UnmanagedBuffer(4))
            {
                bool success;
                while ((success = NativeMethods.K32EnumProcessModulesEx(handle, buffer.Handle, buffer.Size, out int newCb, LIST_MODULES_ALL)) &&
                  buffer.Size != newCb)
                {
                    buffer.Resize(newCb);
                }

                if (!success)
                    return true;

                IntPtr baseAddr = buffer.Read<IntPtr>(0);

                // main module
                int found = IsFound(baseAddr, handle, pathes);

                if (found < 0) // can not get module
                    return true;

                if (found != 0) // kill proc
                {
                    NativeMethods.TerminateProcess(handle, -1);
                    return true;
                }

                int moudleCount = buffer.Size / IntPtr.Size;

                for (int i = 1; i < moudleCount; i++)
                {
                    baseAddr = buffer.Read<IntPtr>(i * IntPtr.Size);
                    found = IsFound(baseAddr, handle, pathes);
                    if (found < 0) // can not get module
                        return true;
                    if (found != 0) // unmap dll
                        NativeMethods.NtUnmapViewOfSection(handle, baseAddr);
                }
                return false;
            }

            int IsFound(IntPtr baseAddr, IntPtr phandle, Dictionary<string, int> targets)
            {
                char[] charBuf = ObjPool.RentCharBuffer();
                int clen = NativeMethods.K32GetMappedFileNameW(phandle, baseAddr, charBuf, charBuf.Length);
                if (clen == 0)
                {
                    ObjPool.ReturnCharBuffer(charBuf);
                    return -1;
                }
                string path = InternelDriveInfo.GetNtPathFromDosPath(charBuf, clen);
                ObjPool.ReturnCharBuffer(charBuf);
                return targets.ContainsKey(path) ? 1 : 0;
            }

        }

        private static bool ReleaseMappedFile(IntPtr handle, Dictionary<string, int> pathes)
        {

            IntPtr start = s_stemMinAppAddress;
            long end = (!Utils.IsWow64Process(handle) ? s_systemMaxAppAddress : s_systemMaxApp32Address).ToInt64();
            long regionSize;
            IntPtr baseAddress;
            string path;
            using (var buffer = new UnmanagedBuffer(MemInfoBufferSizeInt))
            {
                while (start.ToInt64() < end)
                {
                    //MEMORY_BASIC_INFORMATION
                    if (NativeMethods.NtQueryVirtualMemory(handle, start, MemoryBasicInformation, buffer.Handle, s_memInfoBufferSizeIntPtr, out _) != Success)
                        return true;
                    baseAddress = buffer.Read<IntPtr>(0);
                    regionSize = buffer.ReadSizeT(IntPtr.Size * 3);

                    if (buffer.Read<int>(IntPtr.Size * 4) == MEM_Commit &&
                        buffer.Read<int>(IntPtr.Size * 4 + 8) == MEM_Mapped &&
                        NativeMethods.NtQueryVirtualMemory(handle, baseAddress, MemorySectionName, buffer.Handle, s_memInfoBufferSizeIntPtr, out IntPtr len) == Success &&
                        // UNICODE_STRING
                        !string.IsNullOrEmpty(path = InternelDriveInfo.GetNtPathFromDosPath(buffer.Handle + (IntPtr.Size << 1), (len.ToInt32() >> 1) - IntPtr.Size - 1)) &&
                        pathes.ContainsKey(path))
                    {
                        NativeMethods.NtUnmapViewOfSection(handle, baseAddress);
                    }
                    start = new IntPtr(baseAddress.ToInt64() + regionSize);
                }
                return false;
            }
        }

        private static void ReleaseHandle(Dictionary<int, int> handleLocker, Dictionary<string, int> pathes)
        {
            using (var buffer = new UnmanagedBuffer(s_handleInfoSize))
            {
                uint apiRet;
                while ((apiRet = NativeMethods.NtQuerySystemInformation(
                              SystemHandleInformation,
                              buffer.Handle,
                              buffer.Size,
                              out int realLength)) == STATUS_INFO_LENGTH_MISMATCH)
                {
                    buffer.Resize(realLength);
                }
                if (apiRet != Success)
                    return;
                long handleCount = buffer.ReadSizeT(0);

                Parallel.For(0L, handleCount, l =>
                {
                    int pid = buffer.Read<short>((int)(IntPtr.Size + l * s_handleEntrySize));

                    if (handleLocker == null)
                    {
                        if (pid == Utils.AppProcessId)
                            return;
                    }
                    else if (!handleLocker.ContainsKey(pid))
                        return;

                    IntPtr handle = NativeMethods.OpenProcess(ALL_ACCESS, false, pid);
                    if (handle == IntPtr.Zero)
                        return;
                    IntPtr objHandle = new IntPtr(
                               buffer.Read<short>((int)(l * s_handleEntrySize + IntPtr.Size + 6)
                               ));
                    // long access = buffer.ReadSizeT((int)(l * s_handleEntrySize + 8 + IntPtr.Size));
                    IntPtr mh;
                    if ((mh = CopyHandle(handle, objHandle)) == IntPtr.Zero)
                        return;
                    string path;
                    bool isTargetHandle =
                        NativeMethods.GetFileType(mh) == FILE_TYPE_DISK && // is file or dir
                        (path = Utils.GetPathByFileHandle(mh)) != null &&
                        pathes.ContainsKey(path);

                    NativeMethods.CloseHandle(mh);

                    if (isTargetHandle &&
                        (mh = CopyHandle(handle, objHandle, DuplicateCloseSource)) != IntPtr.Zero)
                        NativeMethods.CloseHandle(mh);

                    NativeMethods.CloseHandle(handle);

                    IntPtr CopyHandle(IntPtr sph, IntPtr sh, int option = 0)
                    {
                        return NativeMethods.DuplicateHandle(
                            sph,
                            sh,
                            Utils.AppProcessHandle,
                            out IntPtr myHandle,
                            0,
                            false,
                            DuplicateSameAccess | option) ? myHandle : IntPtr.Zero;
                    }
                });
            }
        }



    }
}
