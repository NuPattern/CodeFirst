namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    // Properties should not have their own individual 
    // Name property, since they are always part of 
    // a component, and their Definition IS their name.
    public interface IProperty : IInstance
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
        /// Resets the property value to its initial value. If a default value 
        /// was specified in the schema, it will be used, as well as a 
        /// value provider, if any.
        /// </summary>
        void Reset();

        /// <summary>
        /// The owning component.
        /// </summary>
        IComponent Owner { get; }

        IPropertySchema Schema { get; }
    }
}