namespace NuPattern.Schema
{
    using System;
    using System.Linq;
    using Xunit;

    public class ToolkitSchemaFixture
    {
        [Fact]
        public void when_creating_product_then_sets_its_toolkit_property()
        {
            var toolkit = new ToolkitSchema("FooToolkit", "1.0");
            var product = toolkit.CreateProductSchema("Product");

            Assert.NotNull(product.Toolkit);
            Assert.Same(toolkit, product.Toolkit);
        }
    }
}