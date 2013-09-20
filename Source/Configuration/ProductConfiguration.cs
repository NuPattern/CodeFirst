namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class ProductConfiguration : ContainerConfiguration
    {
        internal ProductConfiguration(Type productType)
            : base(productType)
        {
        }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.Visit(this);

            return base.Accept(visitor);
        }
    }
}