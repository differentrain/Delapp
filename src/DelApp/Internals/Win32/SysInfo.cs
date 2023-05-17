using System;
using System.Runtime.InteropServices;

namespace DelApp.Internals.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal sealed class SysInfo
    {
        private readonly uint _oemId;

        public readonly int PageSize;

        public readonly IntPtr MinimumApplicationAddress;

        public readonly IntPtr MaximumApplicationAddress;

        public readonly IntPtr ActiveProcessorMask;

        public readonly int NumberOfProcessors;

        public readonly int OrgProcessorType;

        public readonly int AllocationGranularity;

        public readonly short ProcessorLevel;

        public readonly short ProcessorRevision;


    }
}
