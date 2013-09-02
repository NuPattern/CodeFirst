namespace NuPattern.Schema
{
    using System;

    internal class CollectionSchema : ContainerSchema, ICollectionSchema
    {
        public CollectionSchema(string schemaId, ElementSchema itemSchema)
            : base(schemaId)
        {
            itemSchema.Parent = this;
            this.ItemSchema = itemSchema;
        }

        public ElementSchema ItemSchema { get; internal set; }

        public ElementSchema CreateItemSchema(string schemaId)
        {
            return new ElementSchema(schemaId) { Parent = this };
        }

        IElementSchema ICollectionSchema.ItemSchema { get { return ItemSchema; } }

        IElementSchema ICollectionSchema.CreateItemSchema(string schemaId)
        {
            return CreateItemSchema(schemaId);
        }
    }
}