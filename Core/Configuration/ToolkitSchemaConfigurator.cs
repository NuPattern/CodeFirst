namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class ToolkitSchemaConfigurator : ISchemaConfigurator<IToolkitSchema, ToolkitConfiguration>
    {
        public void Configure(IToolkitSchema schema, ToolkitConfiguration configuration)
        {
            Console.WriteLine("Configuring toolkit");
        }
    }
}