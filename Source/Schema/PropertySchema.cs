namespace NuPattern.Schema
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    internal class PropertySchema : InstanceSchema, IPropertySchema
    {
        public string Category { get; set; }

        public bool IsReadOnly { get; set; }

        public Type Type { get; set; }

        public new IComponentSchema Parent
        {
            get { return (IComponentSchema)base.Parent; }
            set { base.Parent = value; }
        }

        // object DefaultValue { get; set; };
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}