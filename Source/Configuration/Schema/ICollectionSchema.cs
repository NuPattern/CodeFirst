namespace NuPattern.Configuration.Schema
{
    using System;

    public interface ICollectionSchema : IContainerSchema
    {
        /// <summary>
        /// Gets the schema of the items contained in this collection.
        /// </summary>
        IElementSchema Item { get; }

        IElementSchema CreateItemSchema(string schemaId);
    }
}