namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public abstract class ContainerConfiguration : ComponentConfiguration
    {
        internal ContainerConfiguration(Type containerType)
            : base(containerType)
        {
        }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            // Something container-specific?
            return base.Accept<TVisitor>(visitor);
        }
    }
}