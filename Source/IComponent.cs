namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IComponent : IInstance, IVisitableInstance, IAnnotated
    {
        event EventHandler<PropertyChangeEventArgs> PropertyChanged;

        IComponentContext Context { get; }

        /// <summary>
        /// The instance name of the component. Should be unique within the product?
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the parent of the current element, or <see langword="null"/> if 
        /// it's the root.
        /// </summary>
        IComponent Parent { get; }

        /// <summary>
        /// Gets the owning product for this component instance.
        /// For a component in an extension product this is the extension product, 
        /// not the extended (parent) product.
        /// For a non-extension product component, this value equals the 
        /// the root product (<see cref="Root" />).
        /// </summary>
        IProduct Product { get; }

        ///// <summary>
        ///// Gets the root product for this instance. Note that for a product, 
        ///// this may be an ancestor product if it has been instantiated as an 
        ///// extension point.
        ///// </summary>
        //IProduct Root { get; }

        IEnumerable<IProperty> Properties { get; }

        /// <summary>
        /// Gets the schema id for this instance.
        /// </summary>
        string SchemaId { get; }

        IComponentInfo Schema { get; }

        IEnumerable<IAutomation> Automations { get; }

        void AddAutomation(IAutomation automation);

        /// <summary>
        /// Smart-casts the component to the specified type. 
        /// </summary>
        /// <typeparam name="T">The type to smart-cast to.</typeparam>
        /// <returns>A valid instance of the given type, if the component is structurally 
        /// compatible with the given type, that is, it contains the at least the same number and 
        /// type of properties and hierarchical structure of descendents; <see langword="null"/> otherwise.</returns>
        T As<T>() where T : class;

        IProperty CreateProperty(string name);
        T Get<T>(string propertyName);
        IComponent Set<T>(string propertyName, T value);
    }
}