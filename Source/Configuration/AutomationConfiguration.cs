namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public abstract class AutomationConfiguration
    {
        public Type ComponentType { get; internal set; }
    }
}