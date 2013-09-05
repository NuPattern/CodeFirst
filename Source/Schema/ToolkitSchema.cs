namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    internal class ToolkitSchema : IToolkitSchema
    {
        public ToolkitSchema(string toolkitId, string toolkitVersion)
            : this(toolkitId, SemanticVersion.Parse(toolkitVersion))
        {
        }

        public ToolkitSchema(string toolkitId, SemanticVersion toolkitVersion)
        {
            Guard.NotNullOrEmpty(() => toolkitId, toolkitId);
            Guard.NotNull(() => toolkitVersion, toolkitVersion);

            var products = new ObservableCollection<ProductSchema>();
            products.CollectionChanged += OnPatternsChanged;

            this.Id = toolkitId;
            this.Version = toolkitVersion;
            this.ProductSchemas = products;
        }

        public string Id { get; private set; }
        public SemanticVersion Version { get; private set; }
        public ICollection<ProductSchema> ProductSchemas { get; private set; }

        public IProductSchema CreateProductSchema(string schemaId)
        {
            var schema  = new ProductSchema(schemaId);
            ProductSchemas.Add(schema);
            return schema;
        }

        IEnumerable<IProductSchema> IToolkitSchema.ProductSchemas { get { return ProductSchemas; } }

        IProductSchema IToolkitSchema.CreateProductSchema(string schemaId)
        {
            return CreateProductSchema(schemaId);
        }

        private void OnPatternsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var product in e.NewItems.OfType<ProductSchema>())
                {
                    product.ToolkitSchema = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var product in e.OldItems.OfType<ProductSchema>())
                {
                    product.ToolkitSchema = null;
                }
            }
        }
    }
}