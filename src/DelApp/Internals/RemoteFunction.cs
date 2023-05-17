//using DelApp.Internals.Win32;
//using Microsoft.Win32.SafeHandles;
//using System;
//using System.CodeDom;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Runtime.InteropServices.ComTypes;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace DelApp.Internals
//{
//    internal static class RemoteFunction
//    {
//        public const int INFINITE_INT = -1;

//        private static readonly string s_ntdllPath32;

//        private static readonly IntPtr s_ldrUnloadDllAnyCpu;



//        static RemoteFunction()
//        {
//            s_ldrUnloadDllAnyCpu = NativeMethods.GetProcAddress(NativeMethods.GetModuleHandle("ntdll.dll"), "LdrUnloadDll");
//            s_ntdllPath32 = FileNDir.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\ntdll.dll");
//            if (Environment.Is64BitProcess)
//            {
//                using (var pe = new PeReader(s_ntdllPath32))
//                {
//                    int[] ints = pe.GetFunctionOffset("LdrUnloadDll");
//                    s_ldrUnloadDllOffset32 = ints.Length != 0 ? ints[0] : 0;
//                }
//                s_ntdllBaseAddress32 = IntPtr.Zero;
//            }
//            else
//            {
//                Any cpu
//                s_ldrUnloadDllOffset32 = -1;
//                s_ntdllBaseAddress32 = s_ldrUnloadDllAnyCpu;
//            }
//        }


//        public static async Task<WaitObjectResult> CallRemoteFunctionAsync<TParam>(IntPtr proc, IntPtr func, TParam arg, int millisecondTimeout = INFINITE_INT)
//            where TParam : unmanaged
//        {
//            return await Task.FromResult(CallRemoteFunction(proc, func, arg, millisecondTimeout)).ConfigureAwait(false);
//        }

//        public static WaitObjectResult CallRemoteFunction<TParam>(IntPtr proc, IntPtr func, TParam arg, int millisecondTimeout = INFINITE_INT)
//            where TParam : unmanaged
//        {
//            unsafe
//            {
//                using (SafeWaitHandle hHandle = NativeMethods.CreateRemoteThread(
//                  proc,  //process handle
//                  IntPtr.Zero,
//                  IntPtr.Zero,
//                  func,  //address
//                  &arg,  // pointer to param
//                  0, out _))
//                {
//                    if (hHandle.IsInvalid)
//                        return WaitObjectResult.WAIT_FAILED;
//                    WaitObjectResult waitResult = NativeMethods.WaitForSingleObject(hHandle.DangerousGetHandle(), millisecondTimeout);
//                    if (waitResult == WaitObjectResult.WAIT_TIMEOUT ||
//                        waitResult == WaitObjectResult.WAIT_ABANDONED)
//                        NativeMethods.TerminateThread(hHandle.DangerousGetHandle(), 0);
//                    return waitResult;

//                }
//            }
//        }


//    }
//}
