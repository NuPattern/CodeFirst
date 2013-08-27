namespace NuPattern.Schema
{
    using System;

    public interface ICollectionSchema : IContainerSchema
    {
        /// <summary>
        /// Gets the schema id of the items contained by 
        /// this collection.
        /// </summary>
        string ItemSchema { get; }
    }
}