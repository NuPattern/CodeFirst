namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public abstract class AutomationConfiguration
    {
        public abstract void Apply(IComponentSchema schema);
    }
}