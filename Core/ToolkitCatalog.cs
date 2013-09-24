namespace NuPattern
{
    using NuPattern.Configuration;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ToolkitCatalog
    {
        private List<IToolkitInfo> toolkits = new List<IToolkitInfo>();
        private IToolkitConfigurationProcessor processor;

        public ToolkitCatalog(IToolkitConfigurationProcessor processor)
        {
            Guard.NotNull(() => processor, processor);

           this.processor = processor;
        }

        public ToolkitCatalog(IToolkitConfigurationProcessor processor, IEnumerable<IToolkitBuilder> builders)
        {
            Guard.NotNull(() => processor, processor);

            this.processor = processor;

            foreach (var builder in builders)
            {
                Add(builder);
            }
        }

        public void Add(IToolkitBuilder builder)
        {
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

            processor.Process(schema, configuration);
            toolkits.Add(schema);
        }

        public IEnumerable<IToolkitInfo> Toolkits { get { return toolkits; } }

        private class ValidatorVisitor : IConfigurationVisitor
        {
            public void Visit<TConfiguration>(TConfiguration configuration)
            {
                Validator.ValidateObject(configuration, new ValidationContext(configuration, null, null), true);
            }
        }
    }
}