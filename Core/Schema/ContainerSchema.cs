namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal class ContainerSchema : ComponentSchema, IContainerSchema
    {
        public ContainerSchema()
        {
            var elements = new ObservableCollection<ComponentSchema>();
            elements.CollectionChanged += OnElementsChanged;
            this.Elements = elements;
        }

        public ICollection<ComponentSchema> Elements { get; private set; }
        public new ComponentSchema Parent { get { return (ComponentSchema)base.Parent; } }

        IEnumerable<IComponentSchema> IContainerSchema.Elements { get { return this.Elements; } }

        //IEnumerable<IExtensionPointSchema> Extensions { get; }

        private void OnElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var component in e.NewItems.OfType<ComponentSchema>())
            {
                component.Parent = this;
            }

            foreach (var component in e.OldItems.OfType<ComponentSchema>())
            {
                component.Parent = null;
            }
        }
    }
}