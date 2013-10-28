namespace NuPattern.Schema
{
    using System;

    public class ElementSchema : ContainerSchema, IElementSchema, IElementInfo
    {
        public ElementSchema(string schemaId)
            : base(schemaId)
        {
        }

        public override bool Accept(ISchemaVisitor visitor)
        {
            if (visitor.VisitEnter(this))
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

            return visitor.VisitLeave(this);
        }
    }
}