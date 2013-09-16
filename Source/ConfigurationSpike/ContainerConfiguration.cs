namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;

    public abstract class ContainerConfiguration : ComponentConfiguration
    {
        internal void Configure(ContainerSchema schema)
        {
            // TODO: do container-specific configuration here.
            base.Configure((ComponentSchema)schema);
        }

        //public IEnumerable<ComponentConfiguration> Elements { get; }

        //IElementBuilder Element(string name);

        //ICollectionBuilder Collection(string name);

        //IEnumerable<IExtensionPointSchema> Extensions { get; }
    }
}