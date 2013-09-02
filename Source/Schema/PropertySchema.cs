namespace NuPattern.Schema
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    internal class PropertySchema : InstanceSchema, IPropertySchema
    {
        public PropertySchema(string propertyName, Type propertyType)
        {
            Guard.NotNullOrEmpty(() => propertyName, propertyName);
            Guard.NotNull(() => propertyType, propertyType);

            this.PropertyName = propertyName;
            this.PropertyType = propertyType;
        }

        public string PropertyName { get; private set; }

        public string Category { get; set; }

        public bool IsReadOnly { get; set; }

        public Type PropertyType { get; set; }

        public new ComponentSchema Parent
        {
            get { return (ComponentSchema)base.Parent; }
            set { base.Parent = value; }
        }

        IComponentSchema IPropertySchema.Parent { get { return Parent; } }

        // object DefaultValue { get; set; };
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}