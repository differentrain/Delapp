using DelApp.Internals.Win32;
using System;
using System.Linq;

namespace DelApp.Internals
{
    //Unfortunately, RM DO NOT Supports folder.

    // The max count of the RestartManager's session is 64.  
    internal sealed class RestartManagerHelper : DisposableSingleton<RestartManagerHelper>
    {
        private const int ERROR_MORE_DATA = 234;

        private readonly uint _handle;

        private RestartManagerHelper()
        {
            if (NativeMethods.RmStartSession(out _handle, 0, Utils.MyGuidStringWithNullChar) != 0)
                _handle = 0;
        }

        public int[] GetHolderList(out RmRebootReason reason, params string[] fileNames)
        {
            reason = RmRebootReason.None;
            if (NativeMethods.RmRegisterResources(
                _handle,
                fileNames.Length, fileNames,
                0, null,
                0, null) != 0)
                return Array.Empty<int>();

            RmProcessInfo[] affectedApps = null;
            uint nCount = 0;
            int err;
            while ((err = NativeMethods.RmGetList(_handle, out var nlength, ref nCount, affectedApps, out reason)) == ERROR_MORE_DATA)
            {
                affectedApps = new RmProcessInfo[nlength];
                nCount = (uint)affectedApps.Length;
            }
            return err == 0 ? affectedApps.Select(rmi => rmi.Process.ProcessId).ToArray() : Array.Empty<int>();
        }


        protected override void DisposeManaged() { }

        protected override void DisposeUnmanaged()
        {
            if (_handle != 0)
                NativeMethods.RmEndSession(_handle);
        }

    }
}
