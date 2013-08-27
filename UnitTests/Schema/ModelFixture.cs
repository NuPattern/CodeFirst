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
            var pattern = new ProductSchema("IAmazonWebServices")
            {
                Properties = 
                {
                    new PropertySchema("AccessKey", typeof(string)),
                    new PropertySchema("SecretKey", typeof(string)),
                },
                Components =
                {
                    new ElementSchema("IStorage")
                    {
                        // TBD: where do we specify the Storage property name?
                        //DisplayName = "Storage",
                        Properties = 
                        {
                            new PropertySchema("RefreshOnLoad", typeof(bool))
                        },
                        Components = 
                        {
                            new CollectionSchema("Buckets")
                            {
                                // TBD: IEnumerable<Buckets> ?
                                Name = "Buckets",
                                //DisplayName = "Buckets",
                                Components = 
                                {
                                    new ElementSchema("IBucket")
                                    {
                                        Properties = 
                                        {
                                            new PropertySchema("Name", typeof(string)),
                                            new PropertySchema("Permissions", typeof(Permissions)),
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