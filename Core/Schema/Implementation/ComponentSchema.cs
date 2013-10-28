namespace NuPattern.Schema
{
    using NuPattern.Configuration;
    using NuPattern.Core.Properties;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    public abstract class ComponentSchema : IComponentSchema, IComponentInfo
    {
        private List<IAutomationSettings> automations = new List<IAutomationSettings>();
        private List<PropertySchema> properties = new List<PropertySchema>();
        private object annotations;

        public ComponentSchema(string schemaId)
        {
            this.SchemaId = schemaId;
            this.DefaultName = schemaId;
            this.CanRename = true;

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

        public bool CanRename { get; set; }

        public string Description { get; set; }

        public string DefaultName { get; set; }

        public string DisplayName { get; set; }

        public bool IsVisible { get; set; }

        public IEnumerable<IPropertySchema> Properties { get { return properties; } }

        public IEnumerable<IAutomationSettings> Automations { get { return automations; } }

        public IPropertySchema CreatePropertySchema(string propertyName, Type propertyType)
        {
            Guard.NotNullOrEmpty(() => propertyName, propertyName);
            Guard.NotNull(() => propertyType, propertyType);

            if (propertyName == "Name")
                throw new ArgumentException(Strings.ComponentSchema.NamePropertyReserved);
            if (Properties.Any(x => x.Name == propertyName))
                throw new ArgumentException(Strings.ComponentSchema.DuplicatePropertyName(propertyName));

            var property = new PropertySchema(propertyName, propertyType, this);
            properties.Add(property);
            return property;
        }

        public void AddAutomation(IAutomationSettings settings)
        {
            automations.Add(settings);
        }

        public abstract bool Accept(ISchemaVisitor visitor);

        #region Annotations

        public void AddAnnotation(object annotation)
        {
            Annotator.AddAnnotation(ref annotations, annotation);
        }

        public object Annotation(Type type)
        {
            return Annotator.Annotation(annotations, type);
        }

        public IEnumerable<object> Annotations(Type type)
        {
            return Annotator.Annotations(annotations, type);
        }

        public void RemoveAnnotations(Type type)
        {
            Annotator.RemoveAnnotations(ref annotations, type);
        }

        #endregion

        IEnumerable<IPropertyInfo> IComponentInfo.Properties { get { return properties; } }
    }
}