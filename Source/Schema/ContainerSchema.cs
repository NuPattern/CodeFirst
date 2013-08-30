namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal abstract class ContainerSchema : ComponentSchema, IContainerSchema
    {
        /// <summary>
        /// Internal constructor used by tests to allow for easy 
        /// functional construction.
        /// </summary>
        internal ContainerSchema(string schemaId)
            : this(schemaId, null)
        {
        }

        public ContainerSchema(string schemaId, ComponentSchema parentSchema)
            : base(schemaId, parentSchema)
        {
            var elements = new ObservableCollection<ComponentSchema>();
            elements.CollectionChanged += OnElementsChanged;
            this.ComponentSchemas = elements;
        }

        public ICollection<ComponentSchema> ComponentSchemas { get; private set; }

        public new ComponentSchema Parent 
        { 
            get { return (ComponentSchema)base.Parent; }
            set { base.Parent = value; }
        }

        public IElementSchema CreateElementSchema(string schemaId)
        {
            return new ElementSchema(schemaId, this);
        }

        public ICollectionSchema CreateCollectionSchema(string schemaId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IComponentSchema> IContainerSchema.ComponentSchemas { get { return this.ComponentSchemas; } }

        IElementSchema IContainerSchema.CreateElementSchema(string schemaId)
        {
            return CreateElementSchema(schemaId);
        }

        ICollectionSchema IContainerSchema.CreateCollectionSchema(string schemaId)
        {
            return CreateCollectionSchema(schemaId);
        }

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