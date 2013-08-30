namespace NuPattern.Schema
{
    using System;
    using System.Linq;
    using Xunit;

    public class ToolkitSchemaFixture
    {
        [Fact]
        public void when_adding_product_to_toolkit_then_sets_its_toolkit_property()
        {
            var schema = new ProductSchema("Product");
            var toolkit = new ToolkitSchema("FooToolkit", "1.0") { ProductSchemas = { schema } };

            Assert.NotNull(schema.ToolkitSchema);
            Assert.Same(toolkit, schema.ToolkitSchema);

            toolkit.ProductSchemas.Remove(schema);

            Assert.Null(schema.Parent);
        }

        [Fact]
        public void when_removing_product_from_toolkit_then_resets_its_toolkit_property()
        {
            var schema = new ProductSchema("Product");
            var toolkit = new ToolkitSchema("FooToolkit", "1.0") { ProductSchemas = { schema } };

            toolkit.ProductSchemas.Remove(schema);

            Assert.Null(schema.Parent);
        }
    }
}