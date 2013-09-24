namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class CollectionSchemaConfigurator : ISchemaConfigurator<ICollectionSchema, CollectionConfiguration>
    {
        public void Configure(ICollectionSchema schema, CollectionConfiguration configuration)
        {
            Console.WriteLine("Configuring collection");
        }
    }
}