namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class PropertyConfiguration : IVisitable
    {
        public Type ComponentType { get; internal set; }

        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IVisitor
        {
            visitor.Visit(this);

            return visitor;
        }
    }
}