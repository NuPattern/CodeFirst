namespace NuPattern.Schema
{
    using System;

    internal class ElementSchema : ContainerSchema, IElementSchema, IElementInfo
    {
        internal ElementSchema(string schemaId)
            : base(schemaId)
        {
        }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.Visit<IElementSchema>(this);
            
            return base.Accept(visitor);
        }
    }
}