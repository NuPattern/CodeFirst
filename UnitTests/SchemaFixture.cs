namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using Xunit;

    public class SchemaFixture
    {
        [Fact]
        public void when_building_schema_hierarchy_then_can_retrieve_root_schema()
        {
            var schema = new ProductSchema("Product")
            {
                Components =
                {
                    new ElementSchema("Element"),
                    new CollectionSchema("Collection", new ElementSchema("Item"))
                    {
                        Components = 
                        {
                            new ElementSchema("Element")
                        }
                    },
                }
            };

            Assert.Null(schema.Parent);
            Assert.Same(schema, schema.Root);
            Assert.Same(schema, schema.Components.First().Root);
            Assert.Same(schema, schema.Components.OfType<ICollectionSchema>().First().Components.First().Root);
            Assert.Same(schema, schema.Components.OfType<ICollectionSchema>().First().Items.Root);
        }

        [Fact]
        public void when_building_schema_hierarchy_then_sets_proper_parents()
        {
            var schema = new ProductSchema("Product")
            {
                Components =
                {
                    new ElementSchema("Element")
                    { 
                        DefaultName = "Foo", 
                        CanRename = false,
                        Properties = 
                        {
                            new PropertySchema("Id", typeof(string)),
                            new PropertySchema("Text", typeof(string)),
                        }
                    },
                    new CollectionSchema("Collection",  new ElementSchema("Item"))
                    {
                        DefaultName = "Bars",
                        CanRename = false,
                        Components = 
                        {
                            new ElementSchema("Element")
                            {
                                Properties = 
                                {
                                    new PropertySchema("Id", typeof(string)),
                                    new PropertySchema("Text", typeof(string)),
                                }
                            }
                        }
                    },
                }
            };

            Assert.Null(schema.Parent);

            var toolkit = new ToolkitSchema("FooToolkit", "1.0") { Products = { schema } };

            Assert.NotNull(schema.Toolkit);
            Assert.Same(toolkit, schema.Toolkit);
            Assert.Equal(1, toolkit.Products.Count);
            Assert.Equal(1, ((IToolkitSchema)toolkit).Products.Count());

            var foo = (IElementSchema)schema.Components.First();
            Assert.Same(schema, foo.Parent);
            Assert.True(foo.Properties.All(p => p.Parent == foo));

            var bars = (ICollectionSchema)schema.Components.Skip(1).First();
            Assert.Same(schema, bars.Parent);
            Assert.True(bars.Components.All(c => c.Parent == bars));

            var bar = bars.Components.First();
            Assert.True(bar.Properties.All(c => c.Parent == bar));
        }
    }
}