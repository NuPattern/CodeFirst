namespace NuPattern.Schema
{
    using NuPattern.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    public class ToolkitSchema : IToolkitSchema, IToolkitInfo
    {
        private List<ProductSchema> products = new List<ProductSchema>();

        public ToolkitSchema(string toolkitId, string toolkitVersion)
            : this(toolkitId, SemanticVersion.Parse(toolkitVersion))
        {
        }

        public ToolkitSchema(string toolkitId, SemanticVersion toolkitVersion)
        {
            Guard.NotNullOrEmpty(() => toolkitId, toolkitId);
            Guard.NotNull(() => toolkitVersion, toolkitVersion);

            this.Id = toolkitId;
            this.Version = toolkitVersion;
        }

        public string Id { get; private set; }
        public SemanticVersion Version { get; private set; }
        public IEnumerable<IProductSchema> Products { get { return products; } }

        public IProductSchema CreateProductSchema(string schemaId)
        {
            var schema = new ProductSchema(schemaId, this);
            products.Add(schema);
            return schema;
        }

        public bool Accept(ISchemaVisitor visitor)
        {
            if (visitor.VisitEnter(this))
            {
                foreach (var product in products)
                {
                    if (!product.Accept(visitor))
                        break;
                }
            }

            return visitor.VisitLeave(this);
        }

        IEnumerable<IProductInfo> IToolkitInfo.Products { get { return products; } }
    }
}