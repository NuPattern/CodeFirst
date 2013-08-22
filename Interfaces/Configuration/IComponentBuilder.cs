namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IComponentBuilder : IInstanceBuilder
    {
        IEnumerable<IPropertyBuilder> Properties { get; }

        IPropertyBuilder Property(string propertyName);

        // string Icon { get; set; }
    }
}