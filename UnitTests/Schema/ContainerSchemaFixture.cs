namespace NuPattern.Schema
{
    using System;
    using System.Linq;
    using Xunit;

    public class ContainerSchemaFixture
    {
        [Fact]
        public void when_adding_component_to_container_then_sets_its_parent_property()
        {
            var parent = new TestContainerSchema("Collection");
            var child = new TestComponentSchema("Item");

            parent.Components.Add(child);

            Assert.NotNull(child.Parent);
            Assert.Same(parent, child.Parent);
        }

        [Fact]
        public void when_removing_component_from_container_then_resets_its_parent_property()
        {
            var parent = new TestContainerSchema("Collection");
            var child = new TestComponentSchema("Item");

            parent.Components.Add(child);
            parent.Components.Remove(child);

            Assert.Null(child.Parent);
        }

        private class TestContainerSchema : ContainerSchema
        {
            public TestContainerSchema(string schemaId)
                : base(schemaId)
            {
            }
        }

        private class TestComponentSchema : ComponentSchema
        {
            public TestComponentSchema(string schemaId)
                : base(schemaId)
            {
            }
        }
    }
}