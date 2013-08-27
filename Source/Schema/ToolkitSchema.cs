namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal class ToolkitSchema : IToolkitSchema
    {
        public ToolkitSchema()
        {
            var patterns = new ObservableCollection<ProductSchema>();
            patterns.CollectionChanged += OnPatternsChanged;
            this.Products = patterns;
        }

        public string Id { get; set; }
        public string Version { get; set; }
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