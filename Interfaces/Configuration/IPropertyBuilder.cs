namespace NuPattern.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public interface IPropertyBuilder : IInstanceBuilder
    {
        IPropertyBuilder Category(string category);

        IPropertyBuilder ReadOnly();

        IPropertyBuilder Typed(Type propertyType);

        new IComponentBuilder Parent { get; }

        // object DefaultValue { get; set; }
        // ValueProvider DefaultValueProvider?
        // TypeConverter TypeConverter { get; set; }
        // ValueProvider ValueProvider { get; set; }
    }
}