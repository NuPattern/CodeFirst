namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal abstract class ComponentSchema : InstanceSchema, IComponentSchema
    {
        public ComponentSchema(string schemaId)
            : this(schemaId, null)
        {
        }

        public ComponentSchema(string schemaId, ComponentSchema parentSchema)
            : base(parentSchema)
        {
            Guard.NotNullOrEmpty(() => schemaId, schemaId);

            var properties = new ObservableCollection<PropertySchema>();
            properties.CollectionChanged += OnPropertiesChanged;

            this.Id = schemaId;
            this.DefaultName = schemaId;
            this.CanRename = true;
            this.PropertySchemas = properties;

            // TODO: see if this behavior needs to be removed from here.
            if (this.DefaultName.IndexOf('.') != -1)
                this.DefaultName = this.DefaultName.Substring(this.DefaultName.LastIndexOf('.') + 1);
            if (this.DefaultName.StartsWith("I"))
                this.DefaultName = this.DefaultName.Substring(1);
        }

        public string Id { get; private set; }
        public string DefaultName { get; set; }
        public bool CanRename { get; set; }
        public ICollection<PropertySchema> PropertySchemas { get; private set; }

        public IPropertySchema CreatePropertySchema(string propertyName, Type propertyType)
        {
            var property = new PropertySchema(propertyName, propertyType);
            PropertySchemas.Add(property);
            return property;
        }

        IEnumerable<IPropertySchema> IComponentSchema.PropertySchemas { get { return this.PropertySchemas; } }

        IPropertySchema IComponentSchema.CreatePropertySchema(string propertyName, Type propertyType)
        {
            return CreatePropertySchema(propertyName, propertyType);
        }

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