namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ToolkitConfiguration : IVisitable
    {
        private Dictionary<Type, ProductConfiguration> products = new Dictionary<Type, ProductConfiguration>();

        public ToolkitConfiguration(string toolkitId, SemanticVersion toolkitVersion)
        {
            Guard.NotNullOrEmpty(() => toolkitId, toolkitId);
            Guard.NotNull(() => toolkitVersion, toolkitVersion);

            this.Identifier = new ToolkitIdentifier(toolkitId, toolkitVersion);
        }

        public IToolkitIdentifier Identifier { get; private set; }

        public IEnumerable<Type> ConfiguredProducts { get { return products.Keys; } }

        public ProductConfiguration Product(Type productType)
        {
            return products.GetOrAdd(productType, type => new ProductConfiguration(type));
        }

        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IVisitor
        {
            visitor.Visit(this);

            foreach (var product in products.Values)
            {
                product.Accept(visitor);
            }

            return visitor;
        }

        private class ToolkitIdentifier : IToolkitIdentifier
        {
            public ToolkitIdentifier(string id, SemanticVersion version)
            {
                this.Id = id;
                this.Version = version;
            }

            public string Id { get; private set; }
            public SemanticVersion Version { get; private set; }
        }
    }
}