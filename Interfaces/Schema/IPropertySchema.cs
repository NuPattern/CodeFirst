namespace NuPattern.Schema
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public interface IPropertySchema : IInstanceSchema
    {
        string Category { get; set; }

        bool IsReadOnly { get; set; }

        Type PropertyType { get; set; }

        new IComponentSchema Parent { get; }

        // object DefaultValue { get; set; }
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}