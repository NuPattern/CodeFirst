namespace NuPattern.ProductStoreFixture
{
    using NuPattern.Schema;
    using NuPattern.Serialization;
    using NuPattern.Tookit.Simple;
    using System;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class given_a_state_file_and_toolkit_schema
    {
        [Fact]
        public void when_action_then_assert()
        {
            var builder = new SimpleModelBuilder();
            var toolkit = builder.Build();

            var product = SchemaMapper.SyncProduct(
                new Product("MyWebService", toolkit.ProductSchemas.First().SchemaId), 
                toolkit.ProductSchemas.First());
            IAmazonWebServices aws = null;

            product.Set<string>(Reflect.GetPropertyName(() => aws.AccessKey), "asdf");
            product.Set<string>(Reflect.GetPropertyName(() => aws.SecretKey), "qwerty");

            var element = product.CreateElement(Reflect.GetPropertyName(() => aws.Storage), 
                toolkit.ProductSchemas.First().ComponentSchemas.First().SchemaId);
            element.Set(Reflect.GetPropertyName(() => aws.Storage.RefreshOnLoad), true);

            var serializer = new JsonProductSerializer();

            serializer.Serialize(Console.Out, new[] { product });
        }

        [Fact]
        public void when_loading_store_then_loads_product_and_associates_schema()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                new[] { new SimpleModelBuilder() });

            store.Load(NullProgress<int>.Default);

            Assert.Equal(1, store.Products.Count());
            Assert.NotNull(store.Products.First().Schema);
        }

        [Fact]
        public void when_loaded_product_deleted_then_removes_from_store()
        {
            var store = new ProductStore(
                new ProductStoreSettings("MySolution", "ProductStoreFixture.Simple.json"),
                new JsonProductSerializer(),
                new[] { new SimpleModelBuilder() });

            store.Load(NullProgress<int>.Default);

            store.Products.First().Delete();

            Assert.Equal(0, store.Products.Count());
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