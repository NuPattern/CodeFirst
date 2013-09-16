namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class EventConfiguration<T> where T : class
    {
        private ComponentConfiguration parent;
        private EventConfiguration configuration;

        internal EventConfiguration(ComponentConfiguration parent, EventConfiguration configuration)
        {
            this.parent = parent;
            this.configuration = configuration;
        }

        public void Execute(Action<T> action)
        {
            //parent.Automations.Add()
        }

        public CommandFor<T> Execute()
        {
            if (configuration.CommandConfiguration == null)
            {
                configuration.CommandConfiguration = new CommandConfiguration();
            }

            return new CommandFor<T>(configuration.CommandConfiguration);
        }
    }
}