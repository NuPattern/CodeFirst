namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    internal class Element : Container, IElement
    {
        public Element(Component parent)
            : base(parent)
        {
        }

        public new IElementSchema Schema { get; set; }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitElement(this);
            return visitor;
        }

        public new IElement Set<T>(string propertyName, T value)
        {
            base.Set(propertyName, value);
            return this;
        }
    }
}