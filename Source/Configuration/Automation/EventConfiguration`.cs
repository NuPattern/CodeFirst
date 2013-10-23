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

        public CommandFor Execute()
        {
            var commandBinding = new BindingConfiguration();
            configuration.CommandBinding = commandBinding;

            return new CommandFor(commandBinding);
        }

        public void Execute(BindingConfiguration configuration)
        {
            this.configuration.CommandBinding = configuration;
        }
    }
}