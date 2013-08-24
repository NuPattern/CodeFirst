namespace NuPattern
{
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
        /// Gets the root pattern ancestor for this instance. Note that for a pattern, 
        /// this may be an ancestor pattern if it has been instantiated as an 
        /// extension point.
        /// </summary>
        IProduct Root { get; }

        IEnumerable<IProperty> Properties { get; }
    }
}