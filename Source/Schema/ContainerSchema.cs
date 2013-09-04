namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal abstract class ContainerSchema : ComponentSchema, IContainerSchema
    {
        internal ContainerSchema(string schemaId)
            : base(schemaId)
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
            var schema = new ElementSchema(schemaId);
            ComponentSchemas.Add(schema);
            return schema;
        }

        public ICollectionSchema CreateCollectionSchema(string schemaId)
        {
            var schema = new CollectionSchema(schemaId);
            ComponentSchemas.Add(schema);
            return schema;
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