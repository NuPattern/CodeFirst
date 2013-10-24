namespace NuPattern
{
    using NuPattern.Core.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class ProductStore : IProductStore, INotifyDisposable
    {
        private ProductStoreSettings settings;
        private IProductSerializer serializer;
        private IToolkitCatalog toolkits;
        private List<Product> products = new List<Product>();

        public event EventHandler Closed = (sender, args) => { };
        public event EventHandler Disposed = (sender, args) => { };

        public ProductStore(
            ProductStoreSettings settings,
            IProductSerializer serializer,
            IToolkitCatalog toolkits)
        {
            Guard.NotNull(() => settings, settings);
            Guard.NotNull(() => serializer, serializer);
            Guard.NotNull(() => toolkits, toolkits);

            this.settings = settings;
            this.serializer = serializer;
            this.toolkits = toolkits;
        }

        public bool IsDisposed { get; private set; }
        public string Name { get { return settings.StoreName; } }

        public IEnumerable<IProduct> Products { get { return products.AsReadOnly(); } }

        public void Close()
        {
            Dispose();
        }

        public IProduct CreateProduct(string name, string toolkitId, string schemaId)
        {
            this.ThrowIfDisposed();

            Guard.NotNullOrEmpty(() => toolkitId, toolkitId);
            Guard.NotNullOrEmpty(() => schemaId, schemaId);

            ThrowIfDuplicate(name);

            var toolkit = toolkits.Find(toolkitId);
            if (toolkit == null)
                throw new ArgumentException(Strings.ProductStore.ToolkitNotFound(toolkitId));

            var schema = toolkit.Products.FirstOrDefault(x => x.SchemaId == schemaId);
            if (schema == null)
                throw new ArgumentException(Strings.ProductStore.ProductSchemaNotFound(schemaId));

            var product = new Product(name, schemaId) { Store = this };
            ComponentMapper.SyncProduct(product, schema);
            products.Add(product);

            return product;
        }

        public void Dispose()
        {
            Clear();
            IsDisposed = true;
            Closed(this, EventArgs.Empty);
            Disposed(this, EventArgs.Empty);
        }

        public void Load(IProgress<int> progress)
        {
            this.ThrowIfDisposed();

            Guard.NotNull(() => progress, progress);

            // TODO: store provides a component scope.

            Clear();
            using (var reader = File.OpenText(settings.StateFile))
            {
                var current = 0;
                foreach (var product in serializer.Deserialize(reader))
                {
                    var toolkit = toolkits.Find(product.Toolkit.Id);

                    // TODO: for each toolkit, its installation path should 
                    // provide another component context + assembly resolution.

                    if (toolkit != null)
                    {
                        if (product.Toolkit.Version < toolkit.Version)
                        {
                            // TODO: implement some version migration if 
                            // provided by the toolkit schema?
                        }

                        var schema = toolkit.Products.FirstOrDefault(x => x.SchemaId == product.SchemaId);
                        if (schema == null)
                        {
                            // TODO: provide some "SchemaMissing" callback 
                            // on the toolkit? Maybe it was just a type rename?
                        }
                        else
                        {
                            ComponentMapper.SyncProduct(product, schema);
                        }
                    }

                    progress.Report(++current);
                    product.Deleted += OnProductDeleted;
                    product.Store = this;
                    products.Add(product);
                }
            }
        }

        public void Save(IProgress<int> progress)
        {
            this.ThrowIfDisposed();

            Guard.NotNull(() => progress, progress);

            using (var writer = new StreamWriter(settings.StateFile, false))
            {
                serializer.Serialize(writer, Report(products, progress));
            }
        }

        private void ThrowIfDuplicate(string name)
        {
            if (products.Any(x => x.Name == name))
                throw new ArgumentException(Strings.ProductStore.DuplicateProductName(name, Name));
        }

        internal void ThrowIfDuplicateRename(string oldName, string newName)
        {
            if (products.Any(c => c.Name == newName))
                throw new ArgumentException(Strings.ProductStore.RenamedDuplicateProduct(oldName, newName, Name));
        }

        private void OnProductDeleted(object sender, EventArgs args)
        {
            var product = (Product)sender;
            products.Remove(product);
            product.Store = null;
            product.Disposed -= OnProductDeleted;
        }

        private void Clear()
        {
            foreach (var product in products.ToArray())
            {
                // Avoids the event calling back for each product.
                product.Disposed -= OnProductDeleted;
                product.Dispose();
            }

            // Remove all on one call.
            products.Clear();
        }

        private IEnumerable<Product> Report(IEnumerable<Product> products, IProgress<int> progress)
        {
            var current = 0;
            foreach (var product in products)
            {
                yield return product;
                progress.Report(++current);
            }
        }
    }
}