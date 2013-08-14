namespace NuPattern
{
    using System;

    public interface IProduct : IContainer
    {
        /// <summary>
        /// Identifier of the toolkit that provided this product.
        /// </summary>
        string Toolkit { get; }

        /// <summary>
        /// Version of the toolkit used to author this product.
        /// </summary>
        string Version { get; }
    }
}