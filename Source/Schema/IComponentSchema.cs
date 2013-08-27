namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IComponentSchema : IInstanceSchema
    {
        /// <summary>
        /// Schema identifier, used on elements created 
        /// based on this schema.
        /// </summary>
        string Id { get; }

        IEnumerable<IPropertySchema> Properties { get; }

        // string Icon { get; set; }
    }
}