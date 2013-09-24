namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public abstract class AutomationConfiguration : IVisitableConfiguration
    {
        public Type ComponentType { get; internal set; }

        public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IConfigurationVisitor;
    }
}