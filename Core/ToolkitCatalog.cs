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
        private IToolkitConfigurationService configurationService;

        public ToolkitCatalog(IToolkitConfigurationService configurationService)
        {
            Guard.NotNull(() => configurationService, configurationService);

           this.configurationService = configurationService;
        }

        public ToolkitCatalog(IToolkitConfigurationService configurationService, IEnumerable<IToolkitBuilder> builders)
        {
            Guard.NotNull(() => configurationService, configurationService);

            this.configurationService = configurationService;

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

            foreach (var product in configuration.ConfiguredProducts)
            {
                schemaBuilder.BuildProduct(schema, product);
            }

            configurationService.Process(schema, configuration);
            toolkits.Add(schema.Id, schema);
        }

        public IToolkitInfo Find(string toolkitId)
        {
            return toolkits.Find(toolkitId);
        }

        private class ValidatorVisitor : IConfigurationVisitor
        {
            public void Visit<TConfiguration>(TConfiguration configuration)
            {
                Validator.ValidateObject(configuration, new ValidationContext(configuration, null, null), true);
            }
        }
    }
}