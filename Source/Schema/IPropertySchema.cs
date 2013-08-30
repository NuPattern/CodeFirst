namespace NuPattern.Schema
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public interface IPropertySchema : IInstanceSchema
    {
        string PropertyName { get; }

        string Category { get; }

        bool IsReadOnly { get; }

        Type PropertyType { get; }

        new IComponentSchema Parent { get; }

        // object DefaultValue { get; set; }
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}