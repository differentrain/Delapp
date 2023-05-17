using System;

namespace DelApp.Internals
{
    internal sealed class HandleInfo
    {
        public readonly IntPtr Handle;
        public readonly long Access;

        private HandleInfo(IntPtr handle, long access) { Handle = handle; Access = access; }



    }
}
