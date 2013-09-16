namespace NuPattern.Configuration
{
    using NuPattern.Configuration.Schema;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public abstract class AutomationConfiguration
    {
        public abstract void Apply(IComponentSchema schema);
    }
}