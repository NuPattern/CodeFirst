namespace NuPattern
{
    using NuPattern.Core.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    public class ProductStore : IProductStore, INotifyDisposable
    {
        private IProductSerializer serializer;
        private IToolkitCatalog toolkits;
        private List<Product> products = new List<Product>();
        private LifetimeEvents<IProduct> productEvents;
        private LifetimeEvents<IComponent> componentEvents;

        private Dictionary<IComponent, IDisposable> eventSubscriptions = new Dictionary<IComponent, IDisposable>();

        public event EventHandler Closed = (sender, args) => { };
        public event EventHandler Loaded = (sender, args) => { };
        public event EventHandler Saved = (sender, args) => { };
        public event EventHandler Disposed = (sender, args) => { };

        public ProductStore(
            string storeFile,
            IProductSerializer serializer,
            IToolkitCatalog toolkits)
        {
            Guard.NotNull(() => storeFile, storeFile);
            Guard.NotNull(() => serializer, serializer);
            Guard.NotNull(() => toolkits, toolkits);

            this.StoreFile = storeFile;
            this.serializer = serializer;
            this.toolkits = toolkits;
            this.productEvents = new LifetimeEvents<IProduct>(this);
            this.componentEvents = new LifetimeEvents<IComponent>(this);
        }

        public LifetimeEvents<IProduct> ProductEvents { get { return productEvents; } }
        public LifetimeEvents<IComponent> ComponentEvents { get { return componentEvents; } }

        public bool IsDisposed { get; private set; }
        public string StoreFile { get; private set; }

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

            productEvents.RaiseCreated(product);
            productEvents.RaiseInstantiated(product);

            return (IProduct)product;
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

            using (var reader = File.OpenText(StoreFile))
            {
                var current = 0;
                foreach (var product in serializer.Deserialize(reader))
                {
                    productEvents.RaiseLoading(product);

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

                            // Raise loading after automation + schema is in place
                            // Raise loaded right after

                            //product.Accept(InstanceVisitor.Create())
                        }
                    }

                    progress.Report(++current);
                    product.Events.Deleting += OnProductDeleting;
                    product.Events.Deleted += OnProductDeleted;
                    product.Store = this;
                    products.Add(product);

                    productEvents.RaiseCreated(product);
                    productEvents.RaiseLoaded(product);
                }
            }

            Loaded(this, EventArgs.Empty);
        }

        public void Save(IProgress<int> progress)
        {
            this.ThrowIfDisposed();

            Guard.NotNull(() => progress, progress);

            using (var writer = new StreamWriter(StoreFile, false))
            {
                serializer.Serialize(writer, Report(products, progress));
            }

            Saved(this, EventArgs.Empty);
        }

        ILifetimeEvents<IProduct> IProductStore.ProductEvents { get { return ProductEvents; } }
        ILifetimeEvents<IComponent> IProductStore.ComponentEvents { get { return ComponentEvents; } }

        private void ThrowIfDuplicate(string name)
        {
            if (products.Any(x => x.Name == name))
                throw new ArgumentException(Strings.ProductStore.DuplicateProductName(name, StoreFile));
        }

        internal void ThrowIfDuplicateRename(string oldName, string newName)
        {
            if (products.Any(c => c.Name == newName))
                throw new ArgumentException(Strings.ProductStore.RenamedDuplicateProduct(oldName, newName, StoreFile));
        }

        private void OnProductDeleting(object sender, EventArgs args)
        {
            productEvents.RaiseDeleting((IProduct)sender);
        }

        private void OnProductDeleted(object sender, EventArgs args)
        {
            var product = (Product)sender;
            products.Remove(product);
            product.Store = null;
            product.Events.Deleting -= OnProductDeleting;
            product.Events.Deleted -= OnProductDeleted;
            productEvents.RaiseDeleted(product);
        }

        private void Clear()
        {
            foreach (var product in products.ToArray())
            {
                // Avoids the event calling back for each product.
                product.Events.Deleted -= OnProductDeleted;
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