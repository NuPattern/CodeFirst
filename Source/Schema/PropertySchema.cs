namespace NuPattern.Schema
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    internal class PropertySchema : InstanceSchema, IPropertySchema
    {
        public PropertySchema(string name, Type type)
        {
            Guard.NotNullOrEmpty(() => name, name);
            Guard.NotNull(() => type, type);

            this.Name = name;
            this.Type = type;
        }

        public string Name { get; private set; }

        public string Category { get; set; }

        public bool IsReadOnly { get; set; }

        public Type Type { get; set; }

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