namespace NuPattern.Schema
{
    using NuPattern.Properties;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal abstract class ComponentSchema : InstanceSchema, IComponentSchema
    {
        private List<IAutomationSettings> automationSettings = new List<IAutomationSettings>();

        public ComponentSchema(string schemaId)
        {
            Guard.NotNullOrEmpty(() => schemaId, schemaId);

            var properties = new ObservableCollection<PropertySchema>();
            properties.CollectionChanged += OnPropertiesChanged;

            this.SchemaId = schemaId;
            this.DefaultName = schemaId;
            this.CanRename = true;
            this.PropertySchemas = properties;

            // TODO: see if this behavior needs to be removed from here.
            if (this.DefaultName.IndexOf('.') != -1)
                this.DefaultName = this.DefaultName.Substring(this.DefaultName.LastIndexOf('.') + 1);
            if (this.DefaultName.StartsWith("I"))
                this.DefaultName = this.DefaultName.Substring(1);
        }

        public string SchemaId { get; private set; }
        public string DefaultName { get; set; }
        public bool CanRename { get; set; }
        public ICollection<PropertySchema> PropertySchemas { get; private set; }

        public PropertySchema CreatePropertySchema(string propertyName, Type propertyType)
        {
            if (propertyName == "Name")
                throw new ArgumentException(Strings.ComponentSchema.NamePropertyReserved);
            if (PropertySchemas.Any(x => x.Name == propertyName))
                throw new ArgumentException(Strings.ComponentSchema.DuplicatePropertyName(propertyName));

            var property = new PropertySchema(propertyName, propertyType);
            PropertySchemas.Add(property);
            return property;
        }

        public IEnumerable<IAutomationSettings> AutomationSettings { get { return automationSettings; } }

        public void AddAutomationSettings(IAutomationSettings settings)
        {
            this.automationSettings.Add(settings);
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