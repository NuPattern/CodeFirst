namespace NuPattern.Configuration
{
    using CommonComposition;
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using NetFx.StringlyTyped;
    using NuPattern.Automation;

    [Component(IsSingleton = true)]
    public class EventAutomationConfigurator : 
        ISchemaConfigurator<IProductSchema, EventConfiguration>,
        ISchemaConfigurator<ICollectionSchema, EventConfiguration>, 
        ISchemaConfigurator<IElementSchema, EventConfiguration>
    {
        private IAutomationSettingsFactory<EventConfiguration, EventAutomationSettings> settingsFactory;

        public EventAutomationConfigurator(IAutomationSettingsFactory<EventConfiguration, EventAutomationSettings> settingsFactory)
        {
            this.settingsFactory = settingsFactory;
        }

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

                schema.AddAutomation(settingsFactory.CreateSettings(configuration));
            }
        }
    }
}