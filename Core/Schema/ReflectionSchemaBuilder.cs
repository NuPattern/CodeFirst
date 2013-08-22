namespace NuPattern.Schema
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal class ReflectionSchemaBuilder
    {
        public IToolkitSchema Build(IToolkitInfo toolkitInfo, Assembly toolkitAssembly)
        {
            return Build(toolkitInfo.Id, toolkitInfo.Version, toolkitAssembly);
        }

        public IToolkitSchema Build(string toolkitId, string toolkitVersion, Assembly toolkitAssembly)
        {
            var builders = toolkitAssembly
                .GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPatternBuilder<>)));

            var schema = new ToolkitSchema
            {
                Id = toolkitId,
                Version = toolkitVersion,
            };

            foreach (var pattern in builders.Select(t => Build(t)))
            {
                if (pattern != null)
                    schema.Patterns.Add(pattern);
            }

            return schema;
        }

        private PatternSchema Build(Type builder)
        {
            return null;
        }
    }
}