namespace NuPattern.Schema
{
    using System;
    using System.Linq;
    using Xunit;

    public class ProductSchemaFixture
    {
        private IProductSchema product;

        public ProductSchemaFixture()
        {
            var toolkit = new ToolkitSchema("Test", "1.0");
            product = toolkit.CreateProductSchema("Product");
        }

        [Fact]
        public void when_create_element_id_is_null_then_throws()
        {
            Assert.Throws<ArgumentNullException>(() => product.CreateElementSchema(null));
        }

        [Fact]
        public void when_create_element_id_is_empty_then_throws()
        {
            Assert.Throws<ArgumentException>(() => product.CreateElementSchema(""));
        }

        [Fact]
        public void when_create_collection_id_is_null_then_throws()
        {
            Assert.Throws<ArgumentNullException>(() => product.CreateCollectionSchema(null));
        }

        [Fact]
        public void when_create_collection_id_is_empty_then_throws()
        {
            Assert.Throws<ArgumentException>(() => product.CreateCollectionSchema(""));
        }

        [Fact]
        public void when_element_id_has_dots_then_default_name_is_last_part()
        {
            var schema = product.CreateElementSchema("Foo.Bar.Baz");

            Assert.Equal("Baz", schema.DefaultName);
        }

        [Fact]
        public void when_element_id_starts_with_I_then_default_name_removes_it()
        {
            var schema = product.CreateElementSchema("IFoo");

            Assert.Equal("Foo", schema.DefaultName);
        }

        [Fact]
        public void when_element_id_has_dots_and_last_part_starts_with_I_then_default_name_is_last_part_without_the_I()
        {
            var schema = product.CreateElementSchema("Foo.Bar.IBaz");

            Assert.Equal("Baz", schema.DefaultName);
        }

        [Fact]
        public void when_adding_property_to_component_then_sets_owner()
        {
            var parent = product.CreateElementSchema("Element");
            var property = parent.CreatePropertySchema("Property", typeof(string));

            Assert.NotNull(property.Owner);
            Assert.Same(parent, property.Owner);
        }

        [Fact]
        public void when_creating_property__then_adds_to_collection()
        {
            var property = product.CreatePropertySchema("Property", typeof(string));

            Assert.Equal(1, product.Properties.Count());
            Assert.Same(property, product.Properties.First());
        }
    }
}