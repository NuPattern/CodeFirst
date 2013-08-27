namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal abstract class ComponentSchema : InstanceSchema, IComponentSchema
    {
        public ComponentSchema(string name)
            : base(name)
        {
            var properties = new ObservableCollection<PropertySchema>();
            properties.CollectionChanged += OnPropertiesChanged;
            this.Properties = properties;
        }

        public string Id { get; set; }
        public ICollection<PropertySchema> Properties { get; private set; }

        IEnumerable<IPropertySchema> IComponentSchema.Properties { get { return this.Properties; } }

        private void OnPropertiesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var property in e.NewItems.OfType<PropertySchema>())
                {
                    property.Parent = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var property in e.OldItems.OfType<PropertySchema>())
                {
                    property.Parent = null;
                }
            }
        }
    }
}