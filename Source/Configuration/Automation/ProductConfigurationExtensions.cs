namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public static class ProductConfigurationExtensions
    {
        public static CommandFor Command(this IToolkitBuilder builder)
        {
            return new CommandFor(new BindingConfiguration());
        }

        public static ProvidedBy ProvidedBy(this IToolkitBuilder builder)
        {
            return NuPattern.Configuration.ProvidedBy.Default;
        }

        public static EventFor<T> OnEvent<T>(this ProductConfiguration<T> configuration)
            where T : class
        {
            return new EventFor<T>(configuration.Configuration);
        }
    }
}