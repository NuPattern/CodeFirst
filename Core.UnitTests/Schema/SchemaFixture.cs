namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using Xunit;

    public class SchemaFixture
    {
        [Fact]
        public void when_creating_name_property_then_throws_because_its_reserved()
        {
            var toolkit = new ToolkitSchema("FooToolkit", "1.0");

            Assert.Throws<ArgumentException>(() =>
                toolkit.CreateProductSchema("IFoo").CreatePropertySchema("Name", typeof(string)));
        }

        [Fact]
        public void when_creating_duplicate_named_property_then_throws()
        {
            var toolkit = new ToolkitSchema("FooToolkit", "1.0");
            var product = toolkit.CreateProductSchema("IFoo");

            product.CreatePropertySchema("IsVisible", typeof(bool));

            Assert.Throws<ArgumentException>(() => product.CreatePropertySchema("IsVisible", typeof(bool)));
        }
    }
}