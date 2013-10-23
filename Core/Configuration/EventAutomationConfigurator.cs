namespace NuPattern.Configuration
{
    using CommonComposition;
    using NuPattern.Automation;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class EventAutomationConfigurator : IComponentConfigurator
    {
        public void Configure(IComponentSchema schema, ComponentConfiguration configuration)
        {
            foreach (var automation in configuration.Automations.OfType<EventConfiguration>())
            {
                schema.AddAutomation(new EventAutomationSettings(
                    automation.EventBinding,
                    new CommandAutomationSettings(automation.CommandBinding)));
            }
        }
    }
}