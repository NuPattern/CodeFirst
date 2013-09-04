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
        string SchemaId { get; }

        /// <summary>
        /// Default instance name for components of this kind.
        /// </summary>
        string DefaultName { get; }

        /// <summary>
        /// Gets a value indicating whether component instances
        /// created from this schema can be named or renamed by 
        /// the user.
        /// </summary>
        bool CanRename { get; }

        IEnumerable<IPropertySchema> PropertySchemas { get; }

        IPropertySchema CreatePropertySchema(string propertyName, Type propertyType);

        // string Icon { get; set; }
    }
}