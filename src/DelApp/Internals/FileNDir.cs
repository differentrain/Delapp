using DelApp.Internals.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DelApp.Internals
{
    internal sealed class FileNDir : IEquatable<FileNDir>
    {
        private const int ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_ACCESS_DENIED = 5;
        private const int ERROR_DIR_NOT_EMPTY = 145;
        private const int ERROR_SHARING_VIOLATION = 0x20;

        private const FileAttributes INVALID_FILE_ATTRIBUTES = (FileAttributes)(-1);
        private const string LongPathPrefix = @"\\?\";
        private const string SearchSuffix = @"\*";

        public FileNDir(string path)
        {
            FullPath = path;
        }

        public string FullPath { get; }

        public bool IsInvalidPath
        {
            get
            {
                unsafe
                {

                    fixed (char* p = FullPath)
                    {
                        char ch = p[FullPath.Length - 1];
                        return ch == '.' || ch == ' ';
                    }
                }
            }
        }

        public FileAttributes Attributes => GetFileAttributes(LongPathPrefix + FullPath);

        public bool Exists => Attributes != 0;

        public bool IsFile => (Attributes & FileAttributes.Directory) == 0;

        public bool IsDrive => InternelDriveInfo.IsDrive(FullPath);

        public FileNDir PreviewDir
        {
            get
            {
                if (InternelDriveInfo.IsDrive(FullPath))
                    return null;

                int length = FullPath.Length - 1;
                unsafe
                {
                    fixed (char* p = FullPath)
                    {
                        while (p[length] != '\\')
                        {
                            --length;
                        }
                        if (p[length - 1] == ':')
                        {
                            ++length;
                        }
                        string dir = new string(p, 0, length);
                        return new FileNDir(dir);
                    }
                }
            }
        }

        public IEnumerable<FileNDir> GetChilds()
        {
            FileAttributes attr = Attributes;
            if (attr == 0 || (attr & FileAttributes.Directory) == 0)
                yield break;
            foreach (var item in GetChildsCore(FullPath))
            {
                yield return item;
            }
        }

        public bool Delete(List<FileNDir> lockedList)
        {
            FileAttributes attr = Attributes;

            if (attr == 0)
                return true;
            else if ((attr & FileAttributes.Directory) != 0)
                return DeleteDirCore(lockedList);
            else
                return DeleteFileNDir(lockedList, NativeMethods.DeleteFileW);
        }

        public bool FastDelete(List<FileNDir> fileList, List<FileNDir> dirList)
        {
            FileAttributes attr = Attributes;
            if (attr == 0)
                return true;
            else if ((attr & FileAttributes.Directory) != 0)
                return DeleteDirCore2(fileList, dirList);
            else
                return DeleteFileNDir(fileList, NativeMethods.DeleteFileW);
        }

        public bool FixPath(out FileNDir newFile)
        {
            newFile = null;

            if (!Exists || !IsInvalidPath)
                return false;

            string npath = GetCorrectPath(FullPath);
            string lpath = LongPathPrefix + npath;
            int i = 2;

            while (GetFileAttributes(lpath) != 0)
            {
                lpath += i;
                npath += i;
                ++i;
            }

            bool ret = NativeMethods.MoveFileW(LongPathPrefix + FullPath, lpath);
            if (ret)
            {
                newFile = new FileNDir(npath);
            }
            return ret;
            string GetCorrectPath(string invalidPath)
            {
                unsafe
                {
                    fixed (char* p = invalidPath)
                    {
                        int pos = invalidPath.Length - 1;
                        while (p[pos] == '.' ||
                               p[pos] == ' ')
                            --pos;

                        var str = new string(p, 0, pos + 1);
                        if (p[pos] == '\\')
                        {
                            return str + $"new_{(IsFile ? "file" : "directory")}";
                        }
                        else
                        {
                            return str;
                        }
                    }
                }
            }
        }


        private bool DeleteDirCore(List<FileNDir> lockedList)
        {
            List<FileNDir> dirs = TraverseDirectory(lockedList);
            bool suc = true;
            int index = dirs.Count - 1;
            while (index >= 0)
            {
                if (!dirs[index--].DeleteFileNDir(lockedList, NativeMethods.RemoveDirectoryW))
                    suc = false;
            }
            ObjPool.ReturnFDList(dirs);
            return suc;

            List<FileNDir> TraverseDirectory(List<FileNDir> mlockedList)
            {
                List<FileNDir> mdirs = ObjPool.RentFDList();
                mdirs.Add(this);
                int i = 0;
                while (i < mdirs.Count)
                {
                    foreach (var item in GetChildsCore(mdirs[i++].FullPath))
                    {
                        if (item.IsFile)
                        {
                            item.DeleteFileNDir(mlockedList, NativeMethods.DeleteFileW);
                        }
                        else
                        {
                            mdirs.Add(item);
                        }
                    }
                }
                return mdirs;
            }
        }

        private bool DeleteDirCore2(List<FileNDir> fileList, List<FileNDir> dirList)
        {
            Parallel.ForEach(GetChilds(), fd =>
            {
                fd.FastDelete(fileList, dirList);
            });
            return DeleteFileNDir(dirList, NativeMethods.RemoveDirectoryW);
        }


        private bool DeleteFileNDir(List<FileNDir> lockedList, Func<string, bool> func)
        {
            var path = LongPathPrefix + FullPath;
            var attr = GetFileAttributes(path);
            if (attr == INVALID_FILE_ATTRIBUTES)
                return true;
            if ((attr & FileAttributes.ReadOnly) != 0)
                NativeMethods.SetFileAttributesW(path, attr - 1);
            if (func(path))
                return true;
            int erro = Marshal.GetLastWin32Error();
            if (erro == ERROR_ACCESS_DENIED || erro == ERROR_SHARING_VIOLATION || erro == ERROR_DIR_NOT_EMPTY)
                lockedList?.Add(this);
            return false;
        }



        public static FileAttributes GetFileAttributes(string path)
        {
            FileAttributes ret = NativeMethods.GetFileAttributesW(path);
            return ret == INVALID_FILE_ATTRIBUTES ? 0 : ret;
        }

        public static string GetFullPath(string path)
        {
            if (path.StartsWith(LongPathPrefix) || InternelDriveInfo.IsDrive(path))
                return null;

            path = LongPathPrefix + path;

            using (var handle = NativeMethods.CreateFileW(
                path,
                FileAccess.Read,
                FileShare.ReadWrite | FileShare.Delete,
                IntPtr.Zero,
                FileMode.Open,
                (FileAttributes)0x02000000, //FILE_FLAG_BACKUP_SEMANTICS 
                IntPtr.Zero))
            {
                if (handle.IsInvalid)
                    return null;
                return Utils.GetPathByFileHandle(handle.DangerousGetHandle());
            }
        }

        private static IEnumerable<FileNDir> GetChildsCore(string path)
        {
            var sc = new SearchContent();
            bool isDrive = InternelDriveInfo.IsDrive(path);
            if (isDrive)
                path = path.Substring(0, path.Length - 1);
            using (FindFileHandle handle = sc.BeginFind(LongPathPrefix + path + SearchSuffix))
            {
                if (handle == null)
                    yield break;

                if (!isDrive)
                    sc.NextFind(handle);
                else
                    yield return new FileNDir($@"{path}\{sc.LastFileName}");

                while (sc.NextFind(handle))
                {
                    string lfn = sc.LastFileName;
                    yield return new FileNDir($@"{path}\{lfn}");
                }
            }
        }


        public bool Equals(FileNDir other) => FullPath == other.FullPath;

        public override bool Equals(object obj) => obj is FileNDir fd && Equals(fd);

        public override int GetHashCode() => FullPath.GetHashCode();

        public override string ToString() => FullPath.ToString();




        private struct SearchContent
        {
            private const int FINDFIRSTEX_INFO_BASIC = 1;
            private const int FINDFIRSTEX_SEARCH_NAME_MATCH = 0;
            private const int FINDFIRSTEX_LARGE_FETCH_ON_DISK_ENTRIES_ONLY = 2 | 4;

            private Win32FindData _findDate;

            public FileAttributes LastFileAttributs => _findDate.FileAttributes;

            public string LastFileName => _findDate.CFileName;

            public FindFileHandle BeginFind(string path)
            {
                FindFileHandle handle = NativeMethods.FindFirstFileExW(
                   path,
                   FINDFIRSTEX_INFO_BASIC,
                   ref _findDate,
                   FINDFIRSTEX_SEARCH_NAME_MATCH,
                   IntPtr.Zero,
                   FINDFIRSTEX_LARGE_FETCH_ON_DISK_ENTRIES_ONLY);

                if (handle.IsInvalid)
                {
                    handle.Dispose();
                    return null;
                }
                return handle;
            }

            public bool NextFind(FindFileHandle findHandle)
            {
                return (NativeMethods.FindNextFileW(findHandle.DangerousGetHandle(), ref _findDate));
            }
        }

    }
}
