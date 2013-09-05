namespace NuPattern.Schema
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ToolkitBuilder : IToolkitBuilder
    {
        private string toolkitId;
        private SemanticVersion toolkitVersion;
        private List<Type> productTypes = new List<Type>();

        public ToolkitBuilder(string toolkitId, SemanticVersion toolkitVersion)
        {
            this.toolkitId = toolkitId;
            this.toolkitVersion = toolkitVersion;
        }

        public IToolkitSchema Build()
        {
            var schema = new ToolkitSchema(toolkitId, toolkitVersion);
            var builder = new SchemaBuilder();
            productTypes.ForEach(t => builder.BuildProduct(schema, t));
            
            return schema;
        }

        public void Product<T>()
        {
            productTypes.Add(typeof(T));
        }
    }
}