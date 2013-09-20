namespace NuPattern.Schema
{
    using System;

    internal class CollectionSchema : ContainerSchema, ICollectionSchema, ICollectionInfo
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

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.Visit<ICollectionSchema>(this);
           
            ItemSchema.Accept(visitor);

            return base.Accept(visitor);
        }

        IElementSchema ICollectionSchema.Item { get { return ItemSchema; } }
        IElementInfo ICollectionInfo.Item { get { return ItemSchema; } }

        IElementSchema ICollectionSchema.CreateItemSchema(string schemaId)
        {
            return CreateItemSchema(schemaId);
        }
    }
}