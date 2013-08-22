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
            var schema = new PatternSchema
            {
                Elements =
                {
                    new ElementSchema 
                    { 
                        Name = "Foo", 
                        Properties = 
                        {
                            new PropertySchema { Name = "Id", Type = typeof(int) },
                            new PropertySchema { Name = "Text", Type = typeof(string) },                            
                        }
                    },
                    new CollectionSchema 
                    {
                        Name = "Bars",
                        Elements = 
                        {
                            new ElementSchema 
                            { 
                                Name = "Bar", 
                                Properties = 
                                {
                                    new PropertySchema { Name = "Id", Type = typeof(int) },
                                    new PropertySchema { Name = "Text", Type = typeof(string) },                            
                                }
                            },
                        }
                    },
                }
            };

            Assert.Null(schema.Parent);
            Assert.Same(schema, schema.Root);
            Assert.Same(schema, schema.Elements.First().Root);
            Assert.Same(schema, schema.Elements.OfType<ICollectionSchema>().First().Elements.First().Root);
        }

        [Fact]
        public void when_building_schema_hierarchy_then_sets_proper_parents()
        {
            var schema = new PatternSchema
            {
                Elements =
                {
                    new ElementSchema 
                    { 
                        Name = "Foo", 
                        Properties = 
                        {
                            new PropertySchema { Name = "Id", Type = typeof(int) },
                            new PropertySchema { Name = "Text", Type = typeof(string) },                            
                        }
                    },
                    new CollectionSchema 
                    {
                        Name = "Bars",
                        Elements = 
                        {
                            new ElementSchema 
                            { 
                                Name = "Bar", 
                                Properties = 
                                {
                                    new PropertySchema { Name = "Id", Type = typeof(int) },
                                    new PropertySchema { Name = "Text", Type = typeof(string) },                            
                                }
                            },
                        }
                    },
                }
            };

            Assert.Null(schema.Parent);

            var foo = (IElementSchema)schema.Elements.First();
            Assert.Same(schema, foo.Parent);
            Assert.True(foo.Properties.All(p => p.Parent == foo));

            var bars = (ICollectionSchema)schema.Elements.Skip(1).First();
            Assert.Same(schema, bars.Parent);
            Assert.True(bars.Elements.All(c => c.Parent == bars));

            var bar = bars.Elements.First();
            Assert.True(bar.Properties.All(c => c.Parent == bar));
        }
    }
}