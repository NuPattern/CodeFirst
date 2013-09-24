namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class PropertyConfiguration : IVisitableConfiguration
    {
        public Type ComponentType { get; internal set; }

        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IConfigurationVisitor
        {
            visitor.Visit(this);

            return visitor;
        }
    }
}