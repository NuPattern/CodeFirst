namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class ToolkitBuilder
    {
        private readonly ToolkitConfiguration configuration = new ToolkitConfiguration();

        public ConfigurationRegistry Configurations { get; private set; }

        public ToolkitBuilder()
        {
            this.Configurations = new ConfigurationRegistry(configuration);
        }

        public virtual IToolkitSchema Build()
        {
            var schema = new ToolkitSchema();

            foreach (var config in configuration.Patterns)
            {
                var pattern = new PatternSchema();
                config.Configure(pattern);

                // Here we'd apply conventions.

                schema.Patterns.Add(pattern);
            }

            return schema;
        }

        public virtual PatternConfiguration<TPattern> Pattern<TPattern>()
        {
            return new PatternConfiguration<TPattern>(configuration.Pattern(typeof(TPattern)));
        }
    }
}