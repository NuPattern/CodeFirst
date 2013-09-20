namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public abstract class AutomationConfiguration : IVisitableConfiguration
    {
        public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IConfigurationVisitor;
    }
}