using System.Runtime.InteropServices;

namespace DelApp.Internals.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ChangeFilterStruct
    {
        public uint CbSize;
        public uint ExtStatus;
    }
}
