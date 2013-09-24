namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;

    public interface IToolkitConfigurationProcessor
    {
        void Process(IToolkitSchema schema, ToolkitConfiguration configuration);
    }
}
