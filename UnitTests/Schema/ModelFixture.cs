namespace NuPattern.Schema
{
    using NuPattern.Tookit.Simple;
    using System;
    using System.Linq;
    using Xunit;

    public class ModelFixture
    {
        [Fact]
        public void when_building_model_then_can_represent_graph()
        {
            var pattern = new ProductSchema
            {
                Name = "IAmazonWebServices",
                Properties = 
                {
                    new PropertySchema
                    {
                         Name = "AccessKey",
                         Type = typeof(string),
                    },
                    new PropertySchema
                    {
                         Name = "SecretKey",
                         Type = typeof(string),
                    },
                },
                Components =
                {
                    new ElementSchema
                    {
                        Name = "IStorage",
                        // TBD: where do we specify the Storage property name?
                        //DisplayName = "Storage",
                        Properties = 
                        {
                            new PropertySchema 
                            {
                                Name = "RefreshOnLoad",
                                Type = typeof(bool),
                            }, 
                        },
                        Components = 
                        {
                            new CollectionSchema
                            {
                                // TBD: IEnumerable<Buckets> ?
                                Name = "Buckets",
                                //DisplayName = "Buckets",
                                Components = 
                                {
                                    new ElementSchema
                                    {
                                        Name = "IBucket",
                                        Properties = 
                                        {
                                            new PropertySchema
                                            {
                                                 Name = "Name",
                                                 Type = typeof(string),
                                            },
                                            new PropertySchema
                                            {
                                                 Name = "Permissions",
                                                 Type = typeof(Permissions),
                                            },
                                        },
                                    }
                                }
                            }
                        }
                    },
                }
            };
        }
    }
}