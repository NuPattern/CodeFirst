namespace NuPattern.Configuration
{
    using CommonComposition;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class PropertySchemaConfigurator : ISchemaConfigurator<IPropertySchema, PropertyConfiguration>
    {
        public void Configure(IPropertySchema schema, PropertyConfiguration configuration)
        {
            Console.WriteLine("Configuring property");
        }
    }
}