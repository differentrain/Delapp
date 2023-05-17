//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Runtime.InteropServices.ComTypes;
//using System.Text;
//using System.Threading.Tasks;

//namespace DelApp.Internals
//{
//    internal class PeReader : IDisposable
//    {
//        private const int BUFFER_SIZE = 4096;

//        private readonly byte[] _buf;

//        private Stream _stream;

//        public PeReader(string dllPath)
//        {
//            _stream = new FileStream(dllPath, FileMode.Open, FileAccess.Read, FileShare.Read);
//            _buf = new byte[BUFFER_SIZE];
//            _stream.Read(_buf, 0, BUFFER_SIZE);
//        }

//        public T Read<T>(long pos)
//            where T : unmanaged
//        {
//            int offset = GetBufferOffset(pos, Marshal.SizeOf<T>());
//            unsafe
//            {
//                fixed (byte* p = _buf)
//                    return *(T*)(p + offset);
//            }
//        }

//        public string ReadStringAnsi(long pos)
//        {
//            the max length of function name is 4096
//            int offset = GetBufferOffset(pos, BUFFER_SIZE);
//            int i = offset;
//            unsafe
//            {
//                fixed (byte* p = _buf)
//                {
//                    while (i < BUFFER_SIZE)
//                    {
//                        if (p[i] == 0)
//                        {
//                            return new string((sbyte*)p, offset, i - offset, Encoding.ASCII);
//                        }
//                        ++i;
//                    }
//                }
//                throw new NotSupportedException();
//            }
//        }

//        public bool EqualsStringAnsi(long pos, string str)
//        {
//            int length = str.Length;
//            int offset = GetBufferOffset(pos, length + 1);
//            unsafe
//            {
//                fixed (byte* ptrBuf = _buf)
//                fixed (char* ptrStr = str)
//                {
//                    int i = 0;
//                    byte* strInByte = ptrBuf + offset;
//                    while (i < length)
//                    {
//                        if (strInByte[i] != ptrStr[i])
//                            return false;
//                        ++i;
//                    }
//                    if (strInByte[i] != 0)
//                        return false;
//                }
//                return true;
//            }
//        }


//        public int[] GetFunctionOffset(params string[] functionNames)
//        {
//            int addressOfNewExeHeader = Read<int>(0x3C);
//            int optionalHeaderOffset = addressOfNewExeHeader + 0x04 + 0x14;
//            ushort magic = Read<ushort>(optionalHeaderOffset);
//            int exportTableOffset = GetOptionalHeaderDataOffset(magic) + optionalHeaderOffset;
//            int exportTableRav = Read<int>(exportTableOffset);
//            int numberOfSections = Read<ushort>(addressOfNewExeHeader + 0x04 + 0x02);
//            int sectionOffset = optionalHeaderOffset + Read<ushort>(addressOfNewExeHeader + 0x04 + 0x10);
//            if (!GetRavRelation(
//                numberOfSections,
//                sectionOffset,
//                rav: exportTableRav,
//                out int va,
//                out int pra,
//                out int foa))
//            {
//                return Array.Empty<int>();
//            }
//            int numOfFunName = Read<int>(foa + 0x18);
//            int addrOfFun = Read<int>(foa + 0x1C);
//            int addrOfFunName = Read<int>(foa + 0x20);
//            int orderOfFun = Read<int>(foa + 0x24);
//            return SearchFuncs(numOfFunName, addrOfFunName, orderOfFun, addrOfFun, va, pra, functionNames);
//        }

//        protected virtual int GetOptionalHeaderDataOffset(ushort magic)
//        {
//            switch (magic)
//            {
//                case 0x10b:
//                    return 0x60;
//                default: //0x20b
//                    return 0x70;
//            }
//        }

//        private bool GetRavRelation(int numberOfSections, int sectionOffset, int rav, out int va, out int pra, out int foa)
//        {
//            pra = foa = 0;
//            sectionOffset += 0x0C;
//            int count = 0;
//            va = Read<int>(sectionOffset);
//            int sra = Read<int>(sectionOffset + 4);
//            while (count < numberOfSections &&
//                   (va + sra) < rav)
//            {
//                sectionOffset += 0x28;
//                va = Read<int>(sectionOffset);
//                sra = Read<int>(sectionOffset + 4);
//                ++count;
//            }
//            if (count == numberOfSections)
//                return false;
//            pra = Read<int>(sectionOffset + 8);
//            foa = rav - va + pra;
//            return true;
//        }

//        private int[] SearchFuncs(int funcCount, int startFuncName, int startFuncOrder, int startFuncAddr, int va, int pra, params string[] functionNames)
//        {
//            int funcFoa;
//            int found = 0;
//            int offset = pra - va;
//            int searchLength = functionNames.Length;
//            int[] result = null;
//            startFuncName += offset;
//            startFuncOrder += offset;
//            startFuncAddr += offset;
//            for (int i = 0; i < funcCount; i++)
//            {
//                funcFoa = Read<int>(startFuncName + (i << 2)) + offset;
//                for (int j = 0; j < searchLength; j++)
//                {
//                    if (EqualsStringAnsi(funcFoa, functionNames[j]))
//                    {
//                        if (result == null)
//                            result = new int[searchLength];

//                        var off = Read<short>(startFuncOrder + (i << 1));
//                        result[j] = Read<int>(startFuncAddr + (off << 2));
//                        ++found;
//                        if (found == searchLength)
//                            return result;
//                    }
//                }
//            }
//            return Array.Empty<int>();
//        }

//        private int GetBufferOffset(long pos, int size)
//        {
//            long streamPos = _stream.Position;
//            if (pos + size > streamPos ||
//                pos < streamPos - BUFFER_SIZE)
//            {
//                _stream.Seek(pos, SeekOrigin.Begin);
//                _stream.Read(_buf, 0, BUFFER_SIZE);
//            }
//            return (int)(pos - _stream.Position + BUFFER_SIZE);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (_stream != null)
//            {
//                if (disposing)
//                    _stream.Dispose();
//                _stream = null;
//            }
//        }

//        public void Dispose()
//        {
//            Dispose(disposing: true);
//            GC.SuppressFinalize(this);
//        }
//    }
//}
