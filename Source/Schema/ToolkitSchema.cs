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

            var patterns = new ObservableCollection<ProductSchema>();
            patterns.CollectionChanged += OnPatternsChanged;
            
            this.Products = patterns;
        }

        public string Id { get; private set; }
        public SemanticVersion Version { get; private set; }

        IEnumerable<IProductSchema> IToolkitSchema.Products { get { return Products; }}

        public ICollection<ProductSchema> Products { get; private set; }

        private void OnPatternsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var pattern in e.NewItems.OfType<ProductSchema>())
                {
                    pattern.Toolkit = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var pattern in e.OldItems.OfType<ProductSchema>())
                {
                    pattern.Toolkit = null;
                }
            }
        }
    }
}