namespace NuPattern.Configuration.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public interface IPropertySchema : IInstanceSchema
    {
        bool IsReadOnly { get; set; }

        IList<Attribute> Attributes { get; }

        string Name { get; }

        Type PropertyType { get; set; }

        new IComponentSchema Parent { get; }

        // object DefaultValue { get; set; }
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}