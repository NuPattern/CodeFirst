namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class CollectionConfiguration : ContainerConfiguration
    {
        internal CollectionConfiguration(Type collectionType)
            : base(collectionType)
        {
        }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.Visit(this);

            return base.Accept(visitor);
        }
    }
}