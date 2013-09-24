namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    // Properties should not have their own individual 
    // Name property, since they are always part of 
    // a component, and their Definition IS their name.
    public interface IProperty : IInstance, IAnnotated
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the typed property value.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// The owning component.
        /// </summary>
        IComponent Owner { get; }

        IPropertyInfo Schema { get; }

        bool ShouldSerializeValue { get; }
    }
}