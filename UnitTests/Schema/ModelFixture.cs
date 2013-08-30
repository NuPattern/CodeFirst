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
                PropertySchemas = 
                {
                    new PropertySchema("AccessKey", typeof(string)),
                    new PropertySchema("SecretKey", typeof(string)),
                },
                ComponentSchemas =
                {
                    new ElementSchema("IStorage")
                    {
                        // TBD: where do we specify the Storage property name?
                        //DisplayName = "Storage",
                        PropertySchemas = 
                        {
                            new PropertySchema("RefreshOnLoad", typeof(bool))
                        },
                        ComponentSchemas = 
                        {
                            // TBD: IEnumerable<Buckets> ?
                            new CollectionSchema("Buckets", new ElementSchema("IBucket")
                                    {
                                        PropertySchemas = 
                                        {
                                            new PropertySchema("Name", typeof(string)),
                                            new PropertySchema("Permissions", typeof(Permissions)),
                                        },
                                    })
                            {
                                DefaultName = "Buckets",
                                //DisplayName = "Buckets",
                            }
                        }
                    },
                }
            };
        }
    }
}