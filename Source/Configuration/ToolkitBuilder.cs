namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class ToolkitBuilder : IToolkitBuilder
    {
        public ToolkitBuilder(string toolkitId, string toolkitVersion)
            : this(toolkitId, SemanticVersion.Parse(toolkitVersion))
        {
        }

        public ToolkitBuilder(string toolkitId, SemanticVersion toolkitVersion)
        {
            this.Configuration = new ToolkitConfiguration(toolkitId, toolkitVersion);
        }

        public ToolkitConfiguration Configuration { get; private set; }

        public ProductConfiguration<T> Product<T>()
            where T : class
        {
            return new ProductConfiguration<T>(Configuration.Product(typeof(T)));
        }
    }
}