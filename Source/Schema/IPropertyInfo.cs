namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public interface IPropertyInfo
    {
        bool IsReadOnly { get; }

        IList<Attribute> Attributes { get; }

        string Name { get; }

        Type PropertyType { get; }

        // object DefaultValue { get; set; }
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}