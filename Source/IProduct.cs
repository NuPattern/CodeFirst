namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    public interface IProduct : IContainer
    {
        new IPatternSchema Schema { get; }

        /// <summary>
        /// Information about the toolkit that provided the schema for this product.
        /// </summary>
        IToolkitInfo Toolkit { get; }

        new IProduct Set<T>(string propertyName, T value);
    }
}