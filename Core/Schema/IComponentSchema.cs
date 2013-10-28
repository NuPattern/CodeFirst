namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IComponentSchema
    {
        string DisplayName { get; }

        string Description { get; }

        bool IsVisible { get; }

        /// <summary>
        /// Schema identifier, used on elements created 
        /// based on this schema.
        /// </summary>
        string SchemaId { get; }

        /// <summary>
        /// Default instance name for components of this kind.
        /// </summary>
        string DefaultName { get; set; }

        /// <summary>
        /// Gets a value indicating whether component instances
        /// created from this schema can be named or renamed by 
        /// the user.
        /// </summary>
        bool CanRename { get; set; }

        IEnumerable<IPropertySchema> Properties { get; }

        IPropertySchema CreatePropertySchema(string propertyName, Type propertyType);

        IEnumerable<IAutomationSettings> Automations { get; }

        void AddAutomation(IAutomationSettings settings);

        bool Accept(ISchemaVisitor visitor);

        // string Icon { get; set; }
    }
}