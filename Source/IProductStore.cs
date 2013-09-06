namespace NuPattern
{
    using System;
    using System.Collections.Generic;

    public interface IProductStore
    {
        string Name { get; }

        IEnumerable<IProduct> Products { get; }

        IProduct CreateProduct(string name, string toolkitId, string schemaId);

        void Load(IProgress<int> progress);
        void Save(IProgress<int> progress);

        ///// <summary>
        ///// Resolves a component at the store scope level.
        ///// </summary>
        //T Resolve<T>();
    }
}