namespace NuPattern.Configuration
{
    using CommonComposition;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class ToolkitConfigurationService : IToolkitConfigurationService
    {
        private IComponentContext context;

        public ToolkitConfigurationService(IComponentContext context)
        {
            Guard.NotNull(() => context, context);

            this.context = context;
        }

        public void Process(IToolkitSchema schema, ToolkitConfiguration configuration)
        {
            schema.Accept(new SchemaVisitor(context, configuration));
        }

        private class SchemaVisitor : IVisitor
        {
            private IComponentContext context;
            private ToolkitConfiguration configuration;

            public SchemaVisitor(IComponentContext context, ToolkitConfiguration configuration)
            {
                this.context = context;
                this.configuration = configuration;
            }

            public void Visit<TSchema>(TSchema schema)
            {
                configuration.Accept(new ConfigurationVisitor<TSchema>(context, schema));
            }
        }

        private class ConfigurationVisitor<TSchema> : IVisitor
        {
            private IComponentContext context;
            private TSchema schema;

            public ConfigurationVisitor(IComponentContext context, TSchema schema)
            {
                this.context = context;
                this.schema = schema;
            }

            public void Visit<TConfiguration>(TConfiguration configuration)
            {
                var configurator = context.ResolveOptional<ISchemaConfigurator<TSchema, TConfiguration>>();
                if (configurator != null)
                    configurator.Configure(schema, configuration);
            }
        }
    }
}