namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class PropertySchema : IPropertySchema, IPropertyInfo
    {
        private object annotations;

        public PropertySchema(string propertyName, Type propertyType, IComponentSchema owner)
        {
            this.Name = propertyName;
            this.PropertyType = propertyType;
            this.Owner = owner;

            this.Attributes = new List<Attribute>();
        }

        public IList<Attribute> Attributes { get; private set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public bool IsVisible { get; set; }

        public string Name { get; private set; }

        public bool IsReadOnly { get; set; }

        public Type PropertyType { get; set; }

        public IComponentSchema Owner { get; private set; }

        public bool Accept(ISchemaVisitor visitor)
        {
            return visitor.VisitProperty(this);
        }

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
    }
}