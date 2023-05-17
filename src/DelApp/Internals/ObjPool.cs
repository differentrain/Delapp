using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DelApp.Internals
{
    internal static class ObjPool
    {
        public const int CharBufferSize = 32767;

        private static readonly ConcurrentBag<char[]> s_chars_pool = new ConcurrentBag<char[]>();
        private static readonly ConcurrentBag<List<FileNDir>> s_stringList_pool = new ConcurrentBag<List<FileNDir>>();

        public static char[] RentCharBuffer() => s_chars_pool.TryTake(out char[] buffer) ? buffer : new char[CharBufferSize];
        public static void ReturnCharBuffer(char[] buffer) => s_chars_pool.Add(buffer);

        public static List<FileNDir> RentFDList() => s_stringList_pool.TryTake(out List<FileNDir> list) ? list : new List<FileNDir>(512);
        public static void ReturnFDList(List<FileNDir> list)
        {
            list.Clear();
            s_stringList_pool.Add(list);
        }

    }
}
