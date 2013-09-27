namespace NuPattern.Automation
{
    using CommonComposition;
    using NuPattern.Configuration;
    using System;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class EventAutomationSettingsFactory : IAutomationSettingsFactory<EventConfiguration, EventAutomationSettings>
    {
        private IAutomationSettingsFactory<ICommandConfiguration, ICommandAutomationSettings> commandFactory;

        public EventAutomationSettingsFactory(IAutomationSettingsFactory<ICommandConfiguration, ICommandAutomationSettings> commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public EventAutomationSettings CreateSettings(EventConfiguration configuration)
        {
            var commandSettings = commandFactory.CreateSettings(configuration.CommandConfiguration);

            return new EventAutomationSettings(configuration.EventType, configuration.EventSettings, commandSettings);
        }
    }
}