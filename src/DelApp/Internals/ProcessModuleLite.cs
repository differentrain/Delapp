//using DelApp.Internals.Win32;
//using Microsoft.Win32.SafeHandles;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace DelApp.Internals
//{
//    internal class ProcessModuleLite
//    {
//        private const int LIST_MODULES_ALL = 0x03;

//        private ProcessModuleLite(int index, IntPtr baseAddr, string path)
//        {
//            Index = index;
//            BaseAddr = baseAddr;
//            Path = path;
//        }
//        public readonly int Index;
//        public readonly IntPtr BaseAddr;
//        public readonly string Path;

//        public override string ToString() => Path;


//        public static IEnumerable<ProcessModuleLite> GetModules(SafeProcessHandle handle)
//        {
//            IntPtr proc = handle.DangerousGetHandle();
//            if (!NativeMethods.K32EnumProcessModulesEx(proc, null, 0, out int cbNeed, LIST_MODULES_ALL))
//            {
//                handle.Dispose();
//                yield break;
//            }
//            int moudleCount = cbNeed / IntPtr.Size;
//            var handleArray = new IntPtr[moudleCount];
//            bool success;
//            while ((success = NativeMethods.K32EnumProcessModulesEx(proc, handleArray, cbNeed, out int newCb, LIST_MODULES_ALL)) &&
//                   cbNeed != newCb)
//            {
//                moudleCount = newCb / IntPtr.Size;
//                handleArray = new IntPtr[moudleCount];
//            }
//            if (!success)
//            {
//                handle.Dispose();
//                yield break;
//            }

//            char[] buffer = ObjPool.RentCharBuffer();
//            IntPtr h;
//            int clen;
//            for (int i = 0; i < moudleCount; i++)
//            {
//                h = handleArray[i];
//                clen = NativeMethods.K32GetMappedFileName(proc, h, buffer, buffer.Length);
//                if (clen == 0)
//                {
//                    handle.Dispose();
//                    ObjPool.ReturnCharBuffer(buffer);
//                    yield break;
//                }
//                yield return new ProcessModuleLite(i, h, InternelDriveInfo.GetNtPathFromDosPath(buffer, clen));
//            }
//            ObjPool.ReturnCharBuffer(buffer);
//        }
//    }
//}
