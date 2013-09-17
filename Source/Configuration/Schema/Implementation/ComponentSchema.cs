namespace NuPattern.Configuration.Schema
{
    using NuPattern.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal abstract class ComponentSchema : InstanceSchema, IComponentSchema, IComponentInfo
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
            this.Properties = properties;

            if (schemaId.StartsWith("System.Collections.Generic.IEnumerable<"))
            {
                this.DefaultName = this.DefaultName.Substring(39);
                this.DefaultName = this.DefaultName.Substring(0, this.DefaultName.Length - 1);

                // TODO: pluralize?
            }

            // TODO: see if this behavior needs to be removed from here.
            if (this.DefaultName.IndexOf('.') != -1)
                this.DefaultName = this.DefaultName.Substring(this.DefaultName.LastIndexOf('.') + 1);
            if (this.DefaultName.StartsWith("I"))
                this.DefaultName = this.DefaultName.Substring(1);
        }

        public string SchemaId { get; private set; }
        public string DefaultName { get; set; }
        public bool CanRename { get; set; }
        public ICollection<PropertySchema> Properties { get; private set; }

        public PropertySchema CreatePropertySchema(string propertyName, Type propertyType)
        {
            if (propertyName == "Name")
                throw new ArgumentException(Strings.ComponentSchema.NamePropertyReserved);
            if (Properties.Any(x => x.Name == propertyName))
                throw new ArgumentException(Strings.ComponentSchema.DuplicatePropertyName(propertyName));

            var property = new PropertySchema(propertyName, propertyType);
            Properties.Add(property);
            return property;
        }

        public IEnumerable<IAutomationSettings> Automations { get { return automationSettings; } }

        public void AddAutomation(IAutomationSettings settings)
        {
            automationSettings.Add(settings);
        }

        IEnumerable<IPropertyInfo> IComponentInfo.Properties { get { return Properties; } }

        IEnumerable<IPropertySchema> IComponentSchema.Properties { get { return Properties; } }

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