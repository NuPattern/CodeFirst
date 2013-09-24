namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class ElementSchemaConfigurator : ISchemaConfigurator<IElementSchema, ElementConfiguration>
    {
        public void Configure(IElementSchema schema, ElementConfiguration configuration)
        {
            Console.WriteLine("Configuring element");
        }
    }
}