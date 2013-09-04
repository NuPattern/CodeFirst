﻿namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public interface IPropertySchema : IInstanceSchema
    {
        bool IsReadOnly { get; }

        IList<Attribute> Attributes { get; }

        string Name { get; }

        Type Type { get; }

        new IComponentSchema Parent { get; }

        // object DefaultValue { get; set; }
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}