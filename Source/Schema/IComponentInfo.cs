namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IComponentInfo
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
        /// Specifies whether component instances created from 
        /// this schema can be named or renamed by the user.
        /// </summary>
        bool CanRename { get; }

        IEnumerable<IPropertyInfo> Properties { get; }

        IEnumerable<IAutomationSettings> Automations { get; }

        // string Icon { get; set; }
    }
}