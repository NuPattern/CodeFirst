namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IComponent : IInstance, INotifyPropertyChanging, INotifyPropertyChanged
    {
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

        /// <summary>
        /// Gets the root pattern ancestor for this instance. Note that for a pattern, 
        /// this may be an ancestor pattern if it has been instantiated as an 
        /// extension point.
        /// </summary>
        IProduct Root { get; }

        IEnumerable<IProperty> Properties { get; }

        /// <summary>
        /// Gets the schema id for this instance.
        /// </summary>
        string SchemaId { get; }

        IComponentSchema Schema { get; }

        IProperty CreateProperty(string name);
        T Get<T>(string propertyName);
        IComponent Set<T>(string propertyName, T value);
    }
}