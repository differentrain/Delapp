using DelApp.Internals.Win32;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DelApp.Internals
{
    internal static class DefaultIcons
    {

        private const uint SIID_FOLDER = 3;
        private const uint SIID_DOCNOASSOC = 0;
        private const uint SIID_FOLDERBACK = 75;
        private const uint SHGSI_ICON = 0x100;


        private const uint SHGSI_SMALLICON = 0x1;


        public static Icon BackSmall { get; } = GetStockIcon(SIID_FOLDERBACK, SHGSI_SMALLICON);
        public static Icon FileSmall { get; } = GetStockIcon(SIID_DOCNOASSOC, SHGSI_SMALLICON);
        public static Icon DirSmall { get; } = GetStockIcon(SIID_FOLDER, SHGSI_SMALLICON);

        public static Icon GetStockIcon(uint type, uint size)
        {
            var info = new SHSTOCKICONINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);

            NativeMethods.SHGetStockIconInfo(type, SHGSI_ICON | size, ref info);
            var icon = (Icon)Icon.FromHandle(info.hIcon).Clone(); // Get a copy that doesn't use the original handle
            NativeMethods.DestroyIcon(info.hIcon); // Clean up native icon to prevent resource leak

            return icon;
        }





    }
}
