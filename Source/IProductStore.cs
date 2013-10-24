namespace NuPattern
{
    using System;
    using System.Collections.Generic;

    public interface IProductStore
    {
        event EventHandler Closed;

        string Name { get; }

        IEnumerable<IProduct> Products { get; }

        void Close();

        IProduct CreateProduct(string name, string toolkitId, string schemaId);

        void Load(IProgress<int> progress);
        void Save(IProgress<int> progress);
    }
}