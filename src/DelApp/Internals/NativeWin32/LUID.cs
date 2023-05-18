using System.Runtime.InteropServices;

namespace DelApp.Internals.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID
    {
        public int LowPart;
        public int HighPart;
    }
}
