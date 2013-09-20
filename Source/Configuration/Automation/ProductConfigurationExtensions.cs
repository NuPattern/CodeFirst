namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public static class ProductConfigurationExtensions
    {
        public static EventFor<T> On<T>(this ProductConfiguration<T> configuration)
            where T : class
        {
            return new EventFor<T>(configuration.Configuration);
        }

        public static CommandFor<T> Command<T>(this ProductConfiguration<T> configuration)
            where T : class
        {
            return new CommandFor<T>(new CommandConfiguration());
        }

        public static ICommandConfiguration Command<T>(this ProductConfiguration<T> configuration, Action<T> command)
            where T : class
        {
            return new AnonymousCommandConfiguration<T>(command);
        }
    }
}