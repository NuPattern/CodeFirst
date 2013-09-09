namespace NuPattern
{
    using System;

    public interface IInstance // IDynamicMetaObjectProvider?
    {
        event EventHandler Deleted;
        void Delete();
    }
}