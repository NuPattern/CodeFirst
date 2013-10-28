namespace NuPattern
{
    using System;

    public interface INotifyDisposable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
