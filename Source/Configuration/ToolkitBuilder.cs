namespace NuPattern.Configuration
{
    using NuPattern.Configuration.Schema;
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class ToolkitBuilder : IToolkitBuilder
    {
        private string toolkitId;
        private SemanticVersion toolkitVersion;
        private ModelConfiguration configuration = new ModelConfiguration();

        public ToolkitBuilder(string toolkitId, string toolkitVersion)
            : this(toolkitId, SemanticVersion.Parse(toolkitVersion))
        {
        }

        public ToolkitBuilder(string toolkitId, SemanticVersion toolkitVersion)
        {
            this.toolkitId = toolkitId;
            this.toolkitVersion = toolkitVersion;
        }

        public IToolkitInfo Build()
        {
            var schema = new ToolkitSchema(toolkitId, toolkitVersion);
            var builder = new SchemaBuilder();

            foreach (var product in configuration.ConfiguredProducts)
            {
                var productSchema = builder.BuildProduct(schema, product);

                configuration.Product(product).Apply(productSchema);
            }
            
            return schema;
        }

        public ProductConfiguration<T> Product<T>()
            where T : class
        {
            return new ProductConfiguration<T>(configuration.Product(typeof(T)));
        }
    }
}