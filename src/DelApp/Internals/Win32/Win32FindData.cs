using System.IO;
using System.Runtime.InteropServices;

namespace DelApp.Internals.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    [BestFitMapping(false)]
    internal unsafe struct Win32FindData
    {
        public readonly FileAttributes FileAttributes;
        private readonly System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
        private readonly System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
        private readonly System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
        private readonly uint FileSizeHigh;
        private readonly uint FileSizeLow;
        private readonly uint dwReserved0;
        private readonly uint dwReserved1;
        private fixed char cFileName[260];
        private fixed char _cAlternateFileName[14];
        internal string CFileName { get { fixed (char* c = cFileName) return new string(c); } }
    }
}
