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
            var schema = new ToolkitSchema("foo", "1.0");

            foreach (var config in configuration.Patterns)
            {
                var pattern = new ProductSchema("foo");
                config.Configure(pattern);

                // Here we'd apply conventions.

                schema.Products.Add(pattern);
            }

            return schema;
        }

        public virtual PatternConfiguration<TPattern> Pattern<TPattern>()
        {
            return new PatternConfiguration<TPattern>(configuration.Pattern(typeof(TPattern)));
        }
    }
}