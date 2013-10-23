namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.ComponentModel;

    public interface IComponentConfigurator
    {
        void Configure(IComponentSchema schema, ComponentConfiguration configuration);
    }
}