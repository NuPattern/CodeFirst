namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class ProductSchemaConfigurator : ISchemaConfigurator<IProductSchema, ProductConfiguration>
    {
        public void Configure(IProductSchema schema, ProductConfiguration configuration)
        {
            Console.WriteLine("Configuring product");
        }
    }
}