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

    public class given_a_state_file_and_toolkit_schema
    {
        [Fact]
        public void when_serializing_product_then_succeeds()
        {
            var builder = new SimpleModelBuilder();
            var toolkit = builder.Build();

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
                new[] { new SimpleModelBuilder() });

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
                new[] { new SimpleModelBuilder() });

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
                new[] { new SimpleModelBuilder() });

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
                new[] { new SimpleModelBuilder() });

            store.CreateProduct("Foo", "SimpleToolkit", typeof(IAmazonWebServices).ToTypeFullName());

            Assert.Throws<ArgumentException>(() =>
                store.CreateProduct("Foo", "SimpleToolkit", typeof(IAmazonWebServices).ToTypeFullName()));
        }
    }

    public class SimpleModelBuilder : ToolkitBuilder
    {
        public SimpleModelBuilder()
            : this("SimpleToolkit", "1.0")
        {
        }

        public SimpleModelBuilder(string toolkitId, string toolkitVersion)
            : base(toolkitId, toolkitVersion)
        {
            base.Product<IAmazonWebServices>();
        }
    }
}