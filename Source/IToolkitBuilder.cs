namespace NuPattern
{
    using NuPattern.Configuration;
    using NuPattern.Schema;
    using System;

    public interface IToolkitBuilder
    {
        ToolkitConfiguration Configuration { get; }
    }
}
