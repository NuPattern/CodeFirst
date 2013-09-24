namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class PropertySchemaConfigurator : ISchemaConfigurator<IPropertySchema, PropertyConfiguration>
    {
        public void Configure(IPropertySchema schema, PropertyConfiguration configuration)
        {
            Console.WriteLine("Configuring property");
        }
    }
}