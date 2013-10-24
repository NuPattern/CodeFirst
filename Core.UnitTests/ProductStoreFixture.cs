namespace NuPattern.ProductStoreFixture
{
    using NuPattern.Schema;
    using NuPattern.Serialization;
    using NuPattern.Tookit.Simple;
    using System;
    using System.Linq;
    using System.Reflection;
    using Xunit;
    using NetFx.StringlyTyped;
    using NuPattern.Configuration;
    using Moq;

    public class given_a_state_file_and_toolkit_schema
    {
        [Fact]
        public void when_serializing_product_then_succeeds()
        {
            var toolkit = Mock.Of<IToolkitInfo>(t => t.Products == new[] 
            { 
                Mock.Of<IProductInfo>(p => 
                    p.Toolkit == Mock.Of<IToolkitInfo>(i => i.Id == "SimpleToolkit" && i.Version == "1.0") &&
                    p.SchemaId == typeof(IAmazonWebServices).FullName && 
                    p.Components == new [] 
                    {
                        Mock.Of<IElementInfo>(e =>
                            e.SchemaId == typeof(IStorage).FullName && 
                            e.DefaultName == "Storage" && 
                            e.Properties == new []
                            {
                                Mock.Of<IPropertyInfo>(ak => ak.Name == "RefreshOnLoad" && ak.PropertyType == typeof(bool)),
                            }
                        )
                    } && 
                    p.Properties == new []
                    {
                        Mock.Of<IPropertyInfo>(ak => ak.Name == "AccessKey" && ak.PropertyType == typeof(string)),
                        Mock.Of<IPropertyInfo>(ak => ak.Name == "SecretKey" && ak.PropertyType == typeof(string)),
                    })
            });

            var product = ComponentMapper.SyncProduct(
                new Product("MyWebService", toolkit.Products.First().SchemaId), 
                toolkit.Products.First());
            IAmazonWebServices aws = null;

            product.Set<string>(Reflect.GetPropertyName(() => aws.AccessKey), "asdf");
            product.Set<string>(Reflect.GetPropertyName(() => aws.SecretKey), "qwerty");

            var element = product.CreateElement(Reflect.GetPropertyName(() => aws.Storage), 
                toolkit.Products.First().Components.First().SchemaId);
            element.Set(Reflect.GetPropertyName(() => aws.Storage.RefreshOnLoad), true);

            var serializer = new JsonProductSerializer();

            serializer.Serialize(Console.Out, new[] { product });
        }

        [Fact]
        public void when_loading_store_then_loads_product_and_associates_schema_and_store()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                Mock.Of<IToolkitCatalog>(c => c.Find("SimpleToolkit") ==
                    Mock.Of<IToolkitInfo>(t => t.Products == new[] 
                    { 
                        Mock.Of<IProductInfo>(p => 
                            p.Toolkit == Mock.Of<IToolkitInfo>() &&
                            p.SchemaId == "NuPattern.Tookit.Simple.IAmazonWebServices")
                    })));

            store.Load(NullProgress<int>.Default);

            Assert.Equal(1, store.Products.Count());
            Assert.NotNull(store.Products.First().Schema);
            Assert.Same(store, store.Products.First().Store);
        }

        [Fact]
        public void when_loaded_product_deleted_then_removes_from_store_and_resets_store()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                Mock.Of<IToolkitCatalog>());

            store.Load(NullProgress<int>.Default);

            var product = store.Products.First();

            product.Delete();

            Assert.Equal(0, store.Products.Count());
            Assert.Null(product.Store);
        }

        [Fact]
        public void when_renaming_product_duplicate_name_then_throws()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                Mock.Of<IToolkitCatalog>(c => c.Find("SimpleToolkit") ==
                    Mock.Of<IToolkitInfo>(t => t.Products == new[] 
                    { 
                        Mock.Of<IProductInfo>(p => 
                            p.Toolkit == Mock.Of<IToolkitInfo>() &&
                            p.SchemaId == "NuPattern.Tookit.Simple.IAmazonWebServices")
                    })));

            store.CreateProduct("Foo", "SimpleToolkit", typeof(IAmazonWebServices).ToTypeFullName());
            var product = store.CreateProduct("Bar", "SimpleToolkit", typeof(IAmazonWebServices).ToTypeFullName());

            Assert.Throws<ArgumentException>(() => product.Name = "Foo");
        }

        [Fact]
        public void when_creating_duplicate_named_product_then_throws()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                Mock.Of<IToolkitCatalog>(c => c.Find("SimpleToolkit") ==
                    Mock.Of<IToolkitInfo>(t => t.Products == new[] 
                    { 
                        Mock.Of<IProductInfo>(p => 
                            p.Toolkit == Mock.Of<IToolkitInfo>() &&
                            p.SchemaId == "NuPattern.Tookit.Simple.IAmazonWebServices")
                    })));

            store.CreateProduct("Foo", "SimpleToolkit", typeof(IAmazonWebServices).ToTypeFullName());

            Assert.Throws<ArgumentException>(() =>
                store.CreateProduct("Foo", "SimpleToolkit", typeof(IAmazonWebServices).ToTypeFullName()));
        }

        [Fact]
        public void when_creating_product_on_disposed_store_then_throws()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                Mock.Of<IToolkitCatalog>());


            store.Dispose();

            Assert.Throws<ObjectDisposedException>(() => store.CreateProduct("foo", "bar", "baz"));
        }

        [Fact]
        public void when_closing_then_raises_closed()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                Mock.Of<IToolkitCatalog>());

            var closed = false;

            store.Closed += (sender, args) => closed = true;

            store.Close();

            Assert.True(closed);
        }

        [Fact]
        public void when_disposing_then_raises_disposed()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                Mock.Of<IToolkitCatalog>());

            var disposed = false;

            store.Disposed += (sender, args) => disposed = true;

            store.Dispose();

            Assert.True(disposed);
        }

        [Fact]
        public void when_closing_then_disposes()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                Mock.Of<IToolkitCatalog>());

            var disposed = false;

            store.Disposed += (sender, args) => disposed = true;

            store.Close();

            Assert.True(disposed);
            Assert.True(store.IsDisposed);
        }

    }
}