using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuPattern
{
    internal interface INotifyDisposable : IDisposable
    {
        event EventHandler Disposed;
        bool IsDisposed { get; }
    }
}
