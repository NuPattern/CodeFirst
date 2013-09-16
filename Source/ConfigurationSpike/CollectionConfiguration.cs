namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;

    public class CollectionConfiguration : ContainerConfiguration, IKeyedConfiguration
    {
        private object key;

        // TODO: should it be internal?
        internal CollectionConfiguration(object key)
        {
            this.key = key;
        }

        object IKeyedConfiguration.Key { get { return this.key; } }

        internal void Configure(CollectionSchema schema)
        {
            // TODO: do element-specific configuration here.
            base.Configure((ContainerSchema)schema);
        }
    }
}