namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class PatternConfiguration : ContainerConfiguration, IKeyedConfiguration
    {
        private object key;

        // TODO: should it be internal?
        internal PatternConfiguration(object key)
        {
            this.key = key;
        }

        object IKeyedConfiguration.Key { get { return this.key; } }

        internal void Configure(ProductSchema schema)
        {
            // TODO: do pattern-specific configuration here.
            base.Configure((ContainerSchema)schema);
        }
    }
}