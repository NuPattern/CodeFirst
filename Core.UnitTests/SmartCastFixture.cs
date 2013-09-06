namespace NuPattern.SmartCastFixture
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using Xunit;

    public class given_a_toolkit
    {
        [Fact]
        public void when_smart_casting_then_succeeds()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();

            var proxy = cast.As<IMyProduct>(product);

            Assert.NotNull(proxy);
            Assert.Equal("asdf", proxy.Key);
            Assert.Equal(product.Name, proxy.Name);
        }

        [Fact]
        public void when_smart_casting_to_derived_then_can_access_base_properties()
        {
            var product = new Product("MyProduct", "IMyProduct");
            product.CreateProperty("Key").Value = "asdf";
            product.CreateProperty("IsVisible").Value = true;
            SchemaMapper.SyncProduct(product, new ProductSchema("IMyProduct")
            {
                PropertySchemas =
                {
                    new PropertySchema("Key", typeof(string)),
                    new PropertySchema("IsVisible", typeof(bool)),
                },
                ToolkitSchema = new ToolkitSchema("MyToolkit", "1.0")
            });

            var cast = new SmartCast();

            var proxy = cast.As<IMyDerivedProduct>(product);

            Assert.NotNull(proxy);
            Assert.Equal("asdf", proxy.Key);
            Assert.Equal(true, proxy.IsVisible);
            Assert.Equal(product.Name, proxy.Name);
        }

        [Fact]
        public void when_converting_proxy_to_component_then_returns_same_instance()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();
            var proxy = cast.As<IMyProduct>(product);
            
            var component = cast.AsComponent(proxy);

            Assert.Same(product, component);
        }

        [Fact]
        public void when_converting_to_interface_with_extra_property_then_returns_null()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();

            var proxy = cast.As<IProductWithAdditionalProperty>(product);

            Assert.Null(proxy);
        }

        [Fact]
        public void when_casting_twice__then_gets_same_proxy_instance()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();

            var proxy1 = cast.As<IMyProduct>(product);
            var proxy2 = cast.As<IMyProduct>(product);

            Assert.Same(proxy1, proxy2);
        }

        [Fact]
        public void when_converting_to_interface_with_incompatible_property_then_returns_null()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();

            var proxy = cast.As<IProductWithIncompatibleProperty>(product);

            Assert.Null(proxy);
        }

        [Fact]
        public void when_setting_property_via_proxy_then_sets_underlying_product_property()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();
            var proxy = cast.As<IMyProduct>(product);

            proxy.Key = "newkey";

            Assert.Equal("newkey", proxy.Key);
            Assert.Equal("newkey", (string)product.Properties.First(x => x.Name == "Key").Value);
        }

        [Fact]
        public void when_setting_name_property_via_proxy_then_renames_component()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();
            var proxy = cast.As<IMyProduct>(product);

            proxy.Name = "FooProduct";

            Assert.Equal("FooProduct", product.Name);
        }

        private static Product InitializeMyProduct()
        {
            var product = new Product("MyProduct", "IMyProduct");
            product.CreateProperty("Key").Value = "asdf";
            SchemaMapper.SyncProduct(product, new ProductSchema("IMyProduct")
            {
                PropertySchemas =
                {
                    new PropertySchema("Key", typeof(string)),
                },
                ToolkitSchema = new ToolkitSchema("MyToolkit", "1.0")
            });
            return product;
        }
    }

    public interface IMyProduct
    {
        string Name { get; set; }
        string Key { get; set; }
    }

    public interface IProductWithIncompatibleProperty
    {
        int Key { get; set; }
    }

    public interface IProductWithAdditionalProperty
    {
        int Port { get; set; }
    }

    public interface IMyDerivedProduct : IMyProduct
    {
        bool IsVisible { get; set; }
    }
}