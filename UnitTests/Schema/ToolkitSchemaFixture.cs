namespace NuPattern.Schema
{
    using NuPattern.Configuration.Schema;
    using System;
    using System.Linq;
    using Xunit;

    public class ToolkitSchemaFixture
    {
        [Fact]
        public void when_adding_product_to_toolkit_then_sets_its_toolkit_property()
        {
            var schema = new ProductSchema("Product");
            var toolkit = new ToolkitSchema("FooToolkit", "1.0") { Products = { schema } };

            Assert.NotNull(schema.Toolkit);
            Assert.Same(toolkit, schema.Toolkit);

            toolkit.Products.Remove(schema);

            Assert.Null(schema.Parent);
        }

        [Fact]
        public void when_removing_product_from_toolkit_then_resets_its_toolkit_property()
        {
            var schema = new ProductSchema("Product");
            var toolkit = new ToolkitSchema("FooToolkit", "1.0") { Products = { schema } };

            toolkit.Products.Remove(schema);

            Assert.Null(schema.Parent);
        }
    }
}