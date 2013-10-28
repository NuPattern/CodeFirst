namespace NuPattern
{
    using System;

    public static class INotifyDisposableExtension
    {
        public static void ThrowIfDisposed(this INotifyDisposable disposable)
        {
            if (disposable.IsDisposed)
                throw new ObjectDisposedException(disposable.ToString());
        }
    }
}
