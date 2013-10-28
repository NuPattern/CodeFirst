namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using Xunit;

    public class ComponentMapperFixture
    {
        [Fact]
        public void when_mapping_product_then_maps_property_schema()
        {
            var toolkit = new ToolkitSchema("Toolkit", "1.0");
            var schema = toolkit.CreateProductSchema("IProduct");
            schema.CreatePropertySchema("IsPublic", typeof(bool));

            var product = new Product("Product", "IProduct");
            product.CreateProperty("IsPublic").Value = true;

            ComponentMapper.SyncProduct(product, (IProductInfo)schema);

            Assert.NotNull(product.Schema);
            Assert.NotNull(product.Properties.First().Schema);
        }

        [Fact]
        public void when_mapping_product_then_removes_properties_that_dont_exist_in_schema()
        {
            var toolkit = new ToolkitSchema("Toolkit", "1.0");
            var schema = toolkit.CreateProductSchema("IProduct");
            schema.CreatePropertySchema("IsPublic", typeof(bool));

            var product = new Product("Product", "IProduct");
            product.CreateProperty("IsVisible").Value = true;

            ComponentMapper.SyncProduct(product, (IProductInfo)schema);

            Assert.NotNull(product.Schema);
            Assert.Equal(1, product.Properties.Count());
            Assert.Equal("IsPublic", product.Properties.First().Name);
            Assert.False((bool)product.Properties.First().Value);
        }

        [Fact]
        public void when_mapping_product_then_does_not_remove_dolar_properties()
        {
            var toolkit = new ToolkitSchema("Toolkit", "1.0");
            var schema = toolkit.CreateProductSchema("IProduct");

            var product = new Product("Product", "IProduct");
            product.CreateProperty("$IsVisible").Value = true;

            ComponentMapper.SyncProduct(product, (IProductInfo)schema);

            Assert.NotNull(product.Schema);
            Assert.Equal(1, product.Properties.Count());
            Assert.Equal("$IsVisible", product.Properties.First().Name);
            Assert.True((bool)product.Properties.First().Value);
        }

        [Fact]
        public void when_mapping_product_then_does_not_remove_underscore_properties()
        {
            var toolkit = new ToolkitSchema("Toolkit", "1.0");
            var schema = toolkit.CreateProductSchema("IProduct");

            var product = new Product("Product", "IProduct");
            product.CreateProperty("_IsVisible").Value = true;

            ComponentMapper.SyncProduct(product, (IProductInfo)schema);

            Assert.NotNull(product.Schema);
            Assert.Equal(1, product.Properties.Count());
            Assert.Equal("_IsVisible", product.Properties.First().Name);
            Assert.True((bool)product.Properties.First().Value);
        }

        [Fact]
        public void when_mapping_product_then_maps_component_schema()
        {
            var toolkit = new ToolkitSchema("Toolkit", "1.0");
            var schema = toolkit.CreateProductSchema("IProduct");
            var element = schema.CreateElementSchema("IElement");
            element.CreatePropertySchema("IsPublic", typeof(bool));

            var product = new Product("Product", "IProduct");
            product.CreateElement("Element", "IElement")
                .CreateProperty("IsPublic").Value = true;

            ComponentMapper.SyncProduct(product, (IProductInfo)schema);

            Assert.NotNull(product.Components.First().Schema);
            Assert.NotNull(product.Components.First().Properties.First().Schema);
        }

        [Fact]
        public void when_mapping_product_then_maps_collection_item_schema()
        {
            var toolkit = new ToolkitSchema("Toolkit", "1.0");
            var schema = toolkit.CreateProductSchema("IProduct");
            var collection = schema.CreateCollectionSchema("ICollection");
            var item = collection.CreateItemSchema("IElement");
            item.CreatePropertySchema("IsPublic", typeof(bool));

            var product = new Product("Product", "IProduct");
            product.CreateCollection("Collection", "ICollection")
                .CreateItem("Element", "IElement")
                .CreateProperty("IsPublic").Value = true;

            ComponentMapper.SyncProduct(product, (IProductInfo)schema);

            Assert.NotNull(product.Components.First().Schema);
            Assert.NotNull(product.Components.OfType<ICollection>().First().Items.First().Schema);
            Assert.NotNull(product.Components.OfType<ICollection>().First().Items.First().Properties.First().Schema);
        }

        [Fact]
        public void when_mapping_product_then_removes_elements_with_no_matching_schema()
        {
            var toolkit = new ToolkitSchema("Toolkit", "1.0");
            var schema = toolkit.CreateProductSchema("IProduct");
            schema.CreateElementSchema("IElement");

            var product = new Product("Product", "IProduct");
            var element = product.CreateElement("Element", "IFoo");

            Assert.Equal(1, product.Components.Count());

            ComponentMapper.SyncProduct(product, (IProductInfo)schema);

            Assert.Equal(0, product.Components.Count());
            Assert.Null(element.Parent);
        }

        [Fact]
        public void when_mapping_product_then_removes_collections_with_no_matching_schema()
        {
            var toolkit = new ToolkitSchema("Toolkit", "1.0");
            var schema = toolkit.CreateProductSchema("IProduct");

            var product = new Product("Product", "IProduct");
            var collection = product.CreateCollection("Collection", "IFoo");

            Assert.Equal(1, product.Components.Count());

            ComponentMapper.SyncProduct(product, (IProductInfo)schema);

            Assert.Equal(0, product.Components.Count());
            Assert.Null(collection.Parent);
        }
    }
}