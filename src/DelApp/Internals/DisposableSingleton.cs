using System;

namespace DelApp.Internals
{
    internal abstract class DisposableSingleton<TSelf> : IDisposable
        where TSelf : DisposableSingleton<TSelf>
    {
        private static TSelf _sharedInstance;
        private bool _disposedValue;

        // the inherited class must and has better has only a private constructor without arguments. 
        public static TSelf Shared
        {
            get
            {
                if (_sharedInstance == null)
                    _sharedInstance = (TSelf)Activator.CreateInstance(typeof(TSelf), true);
                return _sharedInstance;
            }
        }

        // Invoke when app exiting
        public static void ReleaseSharedInstance()
        {
            ((IDisposable)_sharedInstance)?.Dispose();
        }


        protected abstract void DisposeManaged();
        protected abstract void DisposeUnmanaged();

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    DisposeManaged();
                }
                DisposeUnmanaged();
                _disposedValue = true;
            }
        }

        ~DisposableSingleton()
        {
            Dispose(disposing: false);
        }

        // This is a singleton class.
        // So it is better to "hide" Dispose method,
        // to to remind developers that This method should only be invoked when app is closing, or never be invoked.
        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
