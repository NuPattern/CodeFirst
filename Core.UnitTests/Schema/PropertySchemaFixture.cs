namespace NuPattern.Schema
{
    using System;
    using System.Linq;
    using Xunit;

    public class PropertySchemaFixture
    {
        private IProductSchema product;

        public PropertySchemaFixture()
        {
            var toolkit = new ToolkitSchema("Test", "1.0");
            product = toolkit.CreateProductSchema("Product");
        }

        [Fact]
        public void when_property_name_is_null_then_throws()
        {
            Assert.Throws<ArgumentNullException>(() => product.CreatePropertySchema(null, typeof(string)));
        }

        [Fact]
        public void when_property_name_is_empty_then_throws()
        {
            Assert.Throws<ArgumentException>(() => product.CreatePropertySchema("", typeof(string)));
        }

        [Fact]
        public void when_property_type_is_null_then_throws()
        {
            Assert.Throws<ArgumentNullException>(() => product.CreatePropertySchema("Name", null));
        }
    }
}