namespace NuPattern
{
    using System;

    public interface IProduct : IContainer
    {
        /// <summary>
        /// Information about the toolkit that provided the schema for this product.
        /// </summary>
        IToolkitInfo Toolkit { get; }
    }
}