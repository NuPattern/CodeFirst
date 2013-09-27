namespace NuPattern.Configuration
{
    using CommonComposition;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class ProductSchemaConfigurator : ISchemaConfigurator<IProductSchema, ProductConfiguration>
    {
        public void Configure(IProductSchema schema, ProductConfiguration configuration)
        {
            Console.WriteLine("Configuring product");
        }
    }
}