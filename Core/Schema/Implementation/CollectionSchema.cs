namespace NuPattern.Schema
{
    using System;

    public class CollectionSchema : ContainerSchema, ICollectionSchema, ICollectionInfo
    {
        private ElementSchema itemSchema;

        public CollectionSchema(string schemaId)
            : base(schemaId)
        {
        }

        public IElementSchema Item { get { return itemSchema; } }

        public IElementSchema CreateItemSchema(string schemaId)
        {
            return (itemSchema = new ElementSchema(schemaId));
        }

        public override bool Accept(ISchemaVisitor visitor)
        {
            if (visitor.VisitEnter(this))
            {
                if (Item == null || Item.Accept(visitor))
                {
                    foreach (var property in Properties)
                    {
                        if (!property.Accept(visitor))
                            break;
                    }

                    foreach (var component in Components)
                    {
                        if (!component.Accept(visitor))
                            break;
                    }
                }
            }

            return visitor.VisitLeave(this);
        }

        IElementInfo ICollectionInfo.Item { get { return itemSchema; } }
    }
}