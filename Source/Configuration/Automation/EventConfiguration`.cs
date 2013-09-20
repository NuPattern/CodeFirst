namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class EventConfiguration<T> where T : class
    {
        private ComponentConfiguration parent;
        private EventConfiguration configuration;

        public EventConfiguration(ComponentConfiguration parent, EventConfiguration configuration)
        {
            this.parent = parent;
            this.configuration = configuration;
        }

        public void Execute(Action<T> command)
        {
            configuration.CommandConfiguration = new AnonymousCommandConfiguration<T>(command);
        }

        public CommandFor<T> Execute()
        {
            var commandConfig = new CommandConfiguration();
            configuration.CommandConfiguration = commandConfig;

            return new CommandFor<T>(commandConfig);
        }

        public void Execute(ICommandConfiguration command)
        {
            configuration.CommandConfiguration = command;
        }
    }
}