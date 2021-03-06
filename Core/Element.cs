﻿namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    public class Element : Container, IElement
    {
        public Element(string name, string schemaId, Component parent)
            : base(name, schemaId, parent)
        {
            Guard.NotNull(() => parent, parent);
        }

        public new IElementInfo Schema 
        {
            get { return (IElementInfo)base.Schema; }
            set { base.Schema = value; }
        }

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