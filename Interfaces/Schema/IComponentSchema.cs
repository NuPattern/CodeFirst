namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IComponentSchema : IInstanceSchema
    {
        IEnumerable<IPropertySchema> Properties { get; }

        // string Icon { get; set; }
    }
}