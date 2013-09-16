namespace NuPattern.Schema
{
    using System;

    public interface ICollectionInfo : IContainerInfo
    {
        /// <summary>
        /// Gets the schema information of the items contained in this collection.
        /// </summary>
        IElementInfo Item { get; }
    }
}