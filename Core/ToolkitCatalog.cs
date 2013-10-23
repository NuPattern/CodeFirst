namespace NuPattern
{
    using CommonComposition;
    using NuPattern.Configuration;
    using NuPattern.Core.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class ToolkitCatalog : IToolkitCatalog
    {
        private Dictionary<string, IToolkitInfo> toolkits = new Dictionary<string, IToolkitInfo>();
        private List<IComponentConfigurator> configurators;

        public ToolkitCatalog(IEnumerable<IComponentConfigurator> configurators)
            : this(configurators, Enumerable.Empty<IToolkitBuilder>())
        {
        }

        public ToolkitCatalog(IEnumerable<IComponentConfigurator> configurators, IEnumerable<IToolkitBuilder> builders)
        {
            Guard.NotNull(() => configurators, configurators);

            this.configurators = configurators.ToList();

            foreach (var builder in builders)
            {
                Add(builder);
            }
        }

        public IEnumerable<IToolkitInfo> Toolkits { get { return toolkits.Values; } }

        public void Add(IToolkitBuilder builder)
        {
            if (toolkits.ContainsKey(builder.Configuration.Identifier.Id))
                throw new ArgumentException(Strings.ToolkitCatalog.DuplicateSchema(builder.Configuration.Identifier.Id));

            // Validate entire configuration graph before accepting 
            // the builder.
            builder.Configuration.Accept(new ValidatorVisitor());

            var configuration = builder.Configuration;
            var schema = new ToolkitSchema(configuration.Identifier.Id, configuration.Identifier.Version);

            var schemaBuilder = new SchemaBuilder();

            // TODO: what about configured elements and collections? Not necessary?
            foreach (var product in configuration.ConfiguredProducts)
            {
                schemaBuilder.BuildProduct(schema, product);
            }

            var configVisitor = new ConfiguratorVisitor(config =>
                {
                    var componentSchema = schemaBuilder.Map.FindSchema(config.ComponentType);
                    if (componentSchema != null)
                        configurators.ForEach(c => c.Configure(componentSchema, config));
                });

            configuration.Accept(configVisitor);

            toolkits.Add(schema.Id, schema);
        }

        public IToolkitInfo Find(string toolkitId)
        {
            return toolkits.Find(toolkitId);
        }

        private class ConfiguratorVisitor : IVisitor
        {
            private Action<ComponentConfiguration> applyAction;

            public ConfiguratorVisitor(Action<ComponentConfiguration> applyAction)
            {
                this.applyAction = applyAction;
            }

            public void Visit<TConfiguration>(TConfiguration configuration)
            {
                if (typeof(TConfiguration) == typeof(ComponentConfiguration))
                    applyAction.Invoke(configuration as ComponentConfiguration);
            }
        }

        private class ValidatorVisitor : IVisitor
        {
            public void Visit<TConfiguration>(TConfiguration configuration)
            {
                Validator.ValidateObject(configuration, new ValidationContext(configuration, null, null), true);
            }
        }
    }
}