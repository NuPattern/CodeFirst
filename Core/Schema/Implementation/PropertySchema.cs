namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal class PropertySchema : InstanceSchema, IPropertySchema, IPropertyInfo
    {
        public PropertySchema(string propertyName, Type propertyType)
        {
            Guard.NotNullOrEmpty(() => propertyName, propertyName);
            Guard.NotNull(() => propertyType, propertyType);

            this.Name = propertyName;
            this.PropertyType = propertyType;

            this.Attributes = new List<Attribute>();
        }

        public IList<Attribute> Attributes { get; private set; }

        public string Name { get; private set; }

        public bool IsReadOnly { get; set; }

        public Type PropertyType { get; set; }

        public new ComponentSchema Parent
        {
            get { return (ComponentSchema)base.Parent; }
            set { base.Parent = value; }
        }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.Visit<IPropertySchema>(this);
            return visitor;
        }

        IComponentSchema IPropertySchema.Parent { get { return Parent; } }

        // object DefaultValue { get; set; };
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}