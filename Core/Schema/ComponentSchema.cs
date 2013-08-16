namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal class ComponentSchema : InstanceSchema, IComponentSchema
    {
        public ComponentSchema()
        {
            var properties = new ObservableCollection<PropertySchema>();
            properties.CollectionChanged += OnPropertiesChanged;
            this.Properties = properties;
        }

        public ICollection<PropertySchema> Properties { get; private set; }

        IEnumerable<IPropertySchema> IComponentSchema.Properties { get { return this.Properties; } }

        private void OnPropertiesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var property in e.NewItems.OfType<ComponentSchema>())
            {
                property.Parent = this;
            }

            foreach (var property in e.OldItems.OfType<ComponentSchema>())
            {
                property.Parent = null;
            }
        }
    }
}