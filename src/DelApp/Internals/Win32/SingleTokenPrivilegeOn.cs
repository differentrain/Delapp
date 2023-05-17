using System.Runtime.InteropServices;

namespace DelApp.Internals.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal class SingleTokenPrivilegeOn
    {
        public readonly int PrivilegeCount = 1;
        public LUID Luid;
        public readonly int Attributes = 2;
    }
}
