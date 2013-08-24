namespace NuPattern
{
    using System;
    using System.Collections.Generic;

    public interface IStore
    {
        IEnumerable<IProduct> Products { get; }

        void Save();
    }
}