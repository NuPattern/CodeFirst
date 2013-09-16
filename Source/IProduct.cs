namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    public interface IProduct : IContainer
    {
        new IProductInfo Schema { get; }

        /// <summary>
        /// Information about the toolkit that provided the schema for this product.
        /// </summary>
        IToolkitVersion Toolkit { get; }

        /// <summary>
        /// Accesses the store that manages persistence for this product.
        /// </summary>
        IProductStore Store { get; }
        
        new IProduct Set<T>(string propertyName, T value);
    }
}