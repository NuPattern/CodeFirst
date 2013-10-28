namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductStore
    {
        event EventHandler Closed;
        event EventHandler Loaded;
        event EventHandler Saved;

        ILifetimeEvents<IProduct> ProductEvents { get; }
        ILifetimeEvents<IComponent> ComponentEvents { get; }

        string StoreFile { get; }

        IEnumerable<IProduct> Products { get; }

        IProduct CreateProduct(string name, string toolkitId, string schemaId);

        void Close();
        void Load(IProgress<int> progress);
        void Save(IProgress<int> progress);
    }
}