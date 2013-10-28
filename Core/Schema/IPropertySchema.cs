namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public interface IPropertySchema
    {
        string DisplayName { get; }

        string Description { get; }

        bool IsVisible { get; }

        bool IsReadOnly { get; set; }

        IList<Attribute> Attributes { get; }

        string Name { get; }

        Type PropertyType { get; set; }

        IComponentSchema Owner { get; }

        bool Accept(ISchemaVisitor visitor);

        // object DefaultValue { get; set; }
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}