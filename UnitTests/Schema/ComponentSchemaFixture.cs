namespace NuPattern.Schema
{
    using System;
    using System.Linq;
    using Xunit;

    public class ComponentSchemaFixture
    {
        [Fact]
        public void when_id_is_null_then_throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TestComponentSchema(null));
        }

        [Fact]
        public void when_id_is_empty_then_throws()
        {
            Assert.Throws<ArgumentException>(() => new TestComponentSchema(""));
        }

        [Fact]
        public void when_id_has_dots_then_default_name_is_last_part()
        {
            var schema = new TestComponentSchema("Foo.Bar.Baz");

            Assert.Equal("Baz", schema.DefaultName);
        }

        [Fact]
        public void when_id_starts_with_I_then_default_name_removes_it()
        {
            var schema = new TestComponentSchema("IFoo");

            Assert.Equal("Foo", schema.DefaultName);
        }

        [Fact]
        public void when_id_has_dots_and_last_part_starts_with_I_then_default_name_is_last_part_without_the_I()
        {
            var schema = new TestComponentSchema("Foo.Bar.IBaz");

            Assert.Equal("Baz", schema.DefaultName);
        }

        [Fact]
        public void when_adding_property_to_component_then_sets_its_parent_property()
        {
            var parent = new TestComponentSchema("Element");
            var property = new PropertySchema("Property", typeof(string));

            parent.PropertySchemas.Add(property);

            Assert.NotNull(property.Parent);
            Assert.Same(parent, property.Parent);
        }

        [Fact]
        public void when_removing_property_from_component_then_resets_its_parent_property()
        {
            var parent = new TestComponentSchema("Element");
            var property = new PropertySchema("Property", typeof(string));

            parent.PropertySchemas.Add(property);
            parent.PropertySchemas.Remove(property);

            Assert.Null(property.Parent);
        }

        private class TestComponentSchema : ComponentSchema
        {
            public TestComponentSchema(string id)
                : base(id)
            {
            }
        }
    }
}