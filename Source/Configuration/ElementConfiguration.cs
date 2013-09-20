namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class ElementConfiguration : ContainerConfiguration
    {
        internal ElementConfiguration(Type elementType)
            : base(elementType)
        {
        }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.Visit(this);

            return base.Accept(visitor);
        }
    }
}