namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;

    public interface IToolkitConfigurationService
    {
        void Process(IToolkitSchema schema, ToolkitConfiguration configuration);
    }
}
