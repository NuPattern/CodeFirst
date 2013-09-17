namespace NuPattern.Configuration
{
    using NuPattern.Automation;
    using System;
    using System.Linq;

    public class AnonymousCommandConfiguration<T> : ICommandConfiguration
        where T : class
    {
        private Lazy<ICommandAutomationSettings> settings;

        public AnonymousCommandConfiguration(Action<T> command)
        {
            this.settings = new Lazy<ICommandAutomationSettings>(() => new AnonymousCommandAutomationSettings<T>(command));
        }

        public ICommandAutomationSettings CreateSettings()
        {
            return settings.Value;
        }
    }
}