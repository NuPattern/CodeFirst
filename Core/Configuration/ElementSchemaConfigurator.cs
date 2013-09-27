namespace NuPattern.Configuration
{
    using CommonComposition;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class ElementSchemaConfigurator : ISchemaConfigurator<IElementSchema, ElementConfiguration>
    {
        public void Configure(IElementSchema schema, ElementConfiguration configuration)
        {
            Console.WriteLine("Configuring element");
        }
    }
}