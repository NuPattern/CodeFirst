namespace NuPattern.Configuration
{
    using CommonComposition;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class ToolkitSchemaConfigurator : ISchemaConfigurator<IToolkitSchema, ToolkitConfiguration>
    {
        public void Configure(IToolkitSchema schema, ToolkitConfiguration configuration)
        {
            Console.WriteLine("Configuring toolkit");
        }
    }
}