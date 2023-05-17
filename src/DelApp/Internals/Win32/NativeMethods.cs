using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace DelApp.Internals.Win32
{
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
        //==================================== message ================================================

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint message, uint action, in ChangeFilterStruct pChangeFilterStruct);

        //==================================== drag drop ================================================

        [DllImport("shell32", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public unsafe static extern int DragQueryFileW(IntPtr hDrop, uint iFile, char* pszFile, int cch);

        [DllImport("shell32.dll", ExactSpelling = true, SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        public static extern void DragAcceptFiles(IntPtr hWnd, bool fAccept);

        [DllImport("shell32.dll", ExactSpelling = true, SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        public static extern void DragFinish(IntPtr hDrop);

        //==================================== file ================================================

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern FindFileHandle FindFirstFileExW(string lpFileName, int fInfoLevelId, ref Win32FindData lpFindFileData, int fSearchOp, IntPtr lpSearchFilter, int dwAdditionalFlags);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern bool FindNextFileW(IntPtr hFindFile, ref Win32FindData lpFindFileData);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern bool FindClose(IntPtr hFindFile);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern SafeFileHandle CreateFileW(
                string filename,
                FileAccess access,
                FileShare share,
                IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
                FileMode creationDisposition,
                FileAttributes flagsAndAttributes,
                IntPtr templateFile);


        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern FileAttributes GetFileAttributesW(string lpFileName);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern bool MoveFileW(string lpExistingFileName, string lpNewFileName);

        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern int GetFileType(IntPtr hFile);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeleteFileW(string lpPathName);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool RemoveDirectoryW(string lpPathName);

        [DllImport("Kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetFinalPathNameByHandleW(IntPtr hFile, char[] lpszFilePath, int cchFilePath, uint dwFlags);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern bool SetFileAttributesW(string lpFileName, FileAttributes dwFileAttributes);

        //==================================== drives ================================================

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe static extern int QueryDosDeviceW(string lpDeviceName, char* lpTargetPath, int ucchMax);

        //==================================== icon info ================================================

        [DllImport("shell32.dll")]
        public static extern int SHGetStockIconInfo(uint siid, uint uFlags, ref SHSTOCKICONINFO psii);

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr handle);

        //==================================== query object ================================================

        [DllImport("ntdll.dll", ExactSpelling = true, SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        public static extern uint NtQuerySystemInformation(int infoClass, [Out] IntPtr info, int size, out int length);

        //[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        //public static extern uint NtQueryObject(IntPtr objectHandle, int informationClass, [Out] IntPtr informationPtr, int informationLength, out int Length);

        //==================================== environment ================================================
        [DllImport("kernel32", ExactSpelling = true, SetLastError = false)]
        public static extern void GetSystemInfo(SysInfo lpSystemInfo);

        //==================================== process  ================================================
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern IntPtr OpenProcess(int processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("ntdll.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        public static extern uint NtQueryVirtualMemory(IntPtr hProcess, IntPtr lpAddress, int MemoryInformationClass, IntPtr lpBuffer, IntPtr dwLength, out IntPtr size);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        public static extern bool IsWow64Process(IntPtr hProcess, [Out] out bool Wow64Process);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetModuleHandleW(string lpModuleName);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetProcAddress(IntPtr module, string proc);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern bool TerminateProcess(IntPtr processHandle, int exitCode);

        [DllImport("kernel32.dll", ExactSpelling = true, CallingConvention = CallingConvention.Winapi, SetLastError = false)]
        public static extern bool K32EnumProcesses(IntPtr buffer, int bufferSize, out int realSize);

        [DllImport("kernel32.dll", ExactSpelling = true, CallingConvention = CallingConvention.Winapi, SetLastError = false)]
        public static extern bool K32EnumProcessModulesEx(
            [In] IntPtr hProcess,
            [In, Out] IntPtr lphModule,
            [In] int cb,
            [Out] out int lpcbNeeded,
            [In] int dwFilterFlag);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = false)]
        public static extern int K32GetMappedFileNameW([In] IntPtr hProcess, [In] IntPtr hModule, [In, Out] char[] lpFilename, [In] int nSize);

        [DllImport("kernel32.dll", ExactSpelling = true, CallingConvention = CallingConvention.Winapi, SetLastError = false)]
        public static extern int GetProcessId(IntPtr hProcess);

        [DllImport("ntdll.dll", ExactSpelling = true, SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        public static extern uint NtUnmapViewOfSection(IntPtr hProcess, IntPtr baseAddress);


        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern bool OpenProcessToken(IntPtr processHandle, int desiredAccess, out IntPtr tokenHandle);
        [DllImport("advapi32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern bool LookupPrivilegeValueW(string lpSystemName, string lpName, out LUID lpLuid);
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern bool AdjustTokenPrivileges(IntPtr tokenHandle, bool disableAllPrivileges, SingleTokenPrivilegeOn NewState, int bufferLength, IntPtr previousState, IntPtr returnLength);


        // ========================================= thread ===================================================

        //[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        //public static extern unsafe SafeWaitHandle CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, IntPtr dwStackSize, IntPtr lpStartAddress, void* lpParameter, int dwCreationFlags, out int threadID);

        //[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        //public static extern WaitObjectResult WaitForSingleObject([In] IntPtr hHandle, [In] int dwMilliseconds);

        //[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool TerminateThread(IntPtr hThread, int dwExitCode);



        // =========================================restart manager ===================================================

        [DllImport("rstrtmgr.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int RmStartSession(out uint pSessionHandle, uint dwSessionFlags, char[] strSessionKey);
        [DllImport("rstrtmgr.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int RmRegisterResources(uint dwSessionHandle, int nFiles, string[] rgsFilenames, uint nApplications, RmUniqueProcess[] rgApplications, uint nServices, string[] rgsServiceNames);
        [DllImport("rstrtmgr.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo, [Out] RmProcessInfo[] rgAffectedApps, out RmRebootReason lpdwRebootReasons);
        [DllImport("rstrtmgr.dll", ExactSpelling = true)]
        public static extern int RmEndSession(uint pSessionHandle);

    }
}
