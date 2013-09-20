namespace NuPattern.Schema
{
    using System;
    using System.Linq;
    using Xunit;

    public class PropertySchemaFixture
    {
        [Fact]
        public void when_property_name_is_null_then_throws()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertySchema(null, typeof(string)));
        }

        [Fact]
        public void when_property_name_is_empty_then_throws()
        {
            Assert.Throws<ArgumentException>(() => new PropertySchema("", typeof(string)));
        }

        [Fact]
        public void when_property_type_is_null_then_throws()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertySchema("Name", null));
        }
    }
}