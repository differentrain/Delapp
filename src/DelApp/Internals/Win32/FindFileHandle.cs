using Microsoft.Win32.SafeHandles;

namespace DelApp.Internals.Win32
{
    internal sealed class FindFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private FindFileHandle() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.FindClose(handle);
        }
    }
}
