namespace NuPattern
{
    using System;
    using System.Collections.Generic;

    public interface IProductStore
    {
        IEnumerable<IProduct> Products { get; }
        void Save();
    }
}