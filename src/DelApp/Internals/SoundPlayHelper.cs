using Microsoft.Win32;
using System;
using System.IO;
using System.Media;


namespace DelApp.Internals
{
    // The large object heap threshold is 85K which is smaller than ths size of media file.
    // Singleton pattern can avoid repeatedly allocate memory.
    internal sealed class SoundPlayHelper : DisposableSingleton<SoundPlayHelper>
    {
        private SoundPlayer _player;

        private SoundPlayHelper()
        {
            try
            {
                // returns null if not found.
                string soundPath = GetSoundPath();

                if (soundPath != null)
                {
                    _player = new SoundPlayer(soundPath);
                    _player.Load();
                }

             
            }
            catch (Exception ecx)
            {
                Utils.WriteErrorLog(ecx.Message);
                if (_player != null)
                {
                    _player.Dispose();
                    _player = null;
                }
            }
        }


        public void TryPlayEmptyRecyclebin(bool isAsync)
        {
            if (_player != null)
            {
                try
                {
                    if (isAsync)
                        _player.Play();
                    else
                        _player.PlaySync();
                }
                catch (Exception ecx)
                {
                    Utils.WriteErrorLog(ecx.Message);
                }
            }
        }


        protected override void DisposeManaged() => _player?.Dispose();

        protected override void DisposeUnmanaged() => _player = null;


        // returns null if not found.
        private static string GetSoundPath()
        {
            // read path from registery
            using (RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\.Current", false))
            {
                if (reg == null || !(reg.GetValue(null) is string path))
                {
                    // if not found, try "windows\media\Windows Recycle.wav" 
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                    if (path == string.Empty)
                        return null;
                    path += "\\media\\Windows Recycle.wav";
                    return File.Exists(path) ? path : null;
                }
                return path;
            }
        }



    }
}
