namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal abstract class ContainerSchema : ComponentSchema, IContainerSchema, IContainerInfo
    {
        internal ContainerSchema(string schemaId)
            : base(schemaId)
        {
            var elements = new ObservableCollection<ComponentSchema>();
            elements.CollectionChanged += OnElementsChanged;
            this.Components = elements;
        }

        public ICollection<ComponentSchema> Components { get; private set; }

        public new ComponentSchema Parent 
        { 
            get { return (ComponentSchema)base.Parent; }
            set { base.Parent = value; }
        }

        public IElementSchema CreateElementSchema(string schemaId)
        {
            var schema = new ElementSchema(schemaId);
            Components.Add(schema);
            return schema;
        }

        public ICollectionSchema CreateCollectionSchema(string schemaId)
        {
            var schema = new CollectionSchema(schemaId);
            Components.Add(schema);
            return schema;
        }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            foreach (var component in Components)
            {
                component.Accept(visitor);
            }

            return base.Accept<TVisitor>(visitor);
        }

        IEnumerable<IComponentInfo> IContainerInfo.Components { get { return Components; } }
        IEnumerable<IComponentSchema> IContainerSchema.Components { get { return Components; } }

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