namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public abstract class AutomationConfiguration : IVisitable
    {
        public Type ComponentType { get; internal set; }

        public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IVisitor;
    }
}