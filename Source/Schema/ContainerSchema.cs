namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal abstract class ContainerSchema : ComponentSchema, IContainerSchema
    {
        public ContainerSchema(string id)
            : base(id)
        {
            var elements = new ObservableCollection<ComponentSchema>();
            elements.CollectionChanged += OnElementsChanged;
            this.Components = elements;
        }

        public ICollection<ComponentSchema> Components { get; private set; }
        public new ComponentSchema Parent { get { return (ComponentSchema)base.Parent; } }

        IEnumerable<IComponentSchema> IContainerSchema.Components { get { return this.Components; } }

        //IEnumerable<IExtensionPointSchema> Extensions { get; }

        private void OnElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var component in e.NewItems.OfType<ComponentSchema>())
                {
                    component.Parent = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var component in e.OldItems.OfType<ComponentSchema>())
                {
                    component.Parent = null;
                }
            }
        }
    }
}