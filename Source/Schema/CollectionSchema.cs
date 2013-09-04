namespace NuPattern.Schema
{
    using System;

    internal class CollectionSchema : ContainerSchema, ICollectionSchema
    {
        public CollectionSchema(string schemaId)
            : base(schemaId)
        {
        }

        public ElementSchema ItemSchema { get; internal set; }

        public ElementSchema CreateItemSchema(string schemaId)
        {
            ItemSchema = new ElementSchema(schemaId) { Parent = this };
            return ItemSchema;
        }

        IElementSchema ICollectionSchema.ItemSchema { get { return ItemSchema; } }

        IElementSchema ICollectionSchema.CreateItemSchema(string schemaId)
        {
            return CreateItemSchema(schemaId);
        }
    }
}