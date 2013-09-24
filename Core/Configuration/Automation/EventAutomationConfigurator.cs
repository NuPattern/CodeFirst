namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using NetFx.StringlyTyped;
    using NuPattern.Automation;

    public class EventAutomationConfigurator : 
        ISchemaConfigurator<IProductSchema, EventConfiguration>,
        ISchemaConfigurator<ICollectionSchema, EventConfiguration>, 
        ISchemaConfigurator<IElementSchema, EventConfiguration>
    {
        public void Configure(IProductSchema schema, EventConfiguration configuration)
        {
            ConfigureComponent(schema, configuration);
        }

        public void Configure(ICollectionSchema schema, EventConfiguration configuration)
        {
            ConfigureComponent(schema, configuration);
        }

        public void Configure(IElementSchema schema, EventConfiguration configuration)
        {
            ConfigureComponent(schema, configuration);
        }

        private void ConfigureComponent(IComponentSchema schema, EventConfiguration configuration)
        {
            // Only apply the automation to the component whose type id matches the event target.
            if (schema.SchemaId == configuration.ComponentType.ToTypeFullName())
            {
                Console.WriteLine("Configuring event {0} for {1}", configuration.EventType, schema);
                
                
                //schema.AddAutomation(new EventAutomationSettings(configuration.EventType, configuration.EventSettings, 
                //    configuration.)
            }
        }
    }
}