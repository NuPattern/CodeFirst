namespace NuPattern.Configuration
{
    using CommonComposition;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class CollectionSchemaConfigurator : ISchemaConfigurator<ICollectionSchema, CollectionConfiguration>
    {
        public void Configure(ICollectionSchema schema, CollectionConfiguration configuration)
        {
            Console.WriteLine("Configuring collection");
        }
    }
}