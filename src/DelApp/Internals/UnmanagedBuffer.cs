using System;
using System.Runtime.InteropServices;

namespace DelApp.Internals
{
    internal struct UnmanagedBuffer : IDisposable
    {
        private IntPtr _handle;

        public UnmanagedBuffer(int size)
        {
            _handle = Marshal.AllocHGlobal(size);
            Size = size;
        }

        public IntPtr Handle => _handle;

        public int Size { get; private set; }

        public void Resize(int size)
        {
            Marshal.FreeHGlobal(_handle);
            _handle = Marshal.AllocHGlobal(size);
            Size = size;
        }

        public T Read<T>(int offsetInBytes)
            where T : unmanaged
        {
            unsafe
            {
                var ptr = (byte*)_handle.ToPointer() + offsetInBytes;
                return *(T*)ptr;
            }
        }

        public long ReadSizeT(int offsetInBytes)
        {
            unsafe
            {
                var ptr = (byte*)_handle.ToPointer() + offsetInBytes;
                return (*(IntPtr*)ptr).ToInt64();
            }
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(_handle);
        }
    }
}
