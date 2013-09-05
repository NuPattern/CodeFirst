namespace NuPattern
{
    using System;

    public interface IInstance // IDynamicMetaObjectProvider?
    {
        event EventHandler Disposed;
        void Delete();
    }
}