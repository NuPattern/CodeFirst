namespace NuPattern.Schema
{
    using System;

    public interface ICollectionSchema : IContainerSchema
    {
        /// <summary>
        /// Gets the schema of the items contained in this collection.
        /// </summary>
        IElementSchema ItemSchema { get; }

        IElementSchema CreateItemSchema(string schemaId);
    }
}