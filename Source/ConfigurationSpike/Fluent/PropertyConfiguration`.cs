namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class PropertyConfiguration<TProperty> : ConfigurationBase
    {
        private PropertyConfiguration configuration;

        internal PropertyConfiguration(PropertyConfiguration configuration)
        {
            this.configuration = configuration;
            configuration.PropertyType = typeof(TProperty);
        }

        public PropertyConfiguration<TProperty> Hidden()
        {
            this.configuration.Hidden = true;
            return this;
        }
    }
}