namespace NuPattern.Configuration
{
    using NuPattern.Configuration.Schema;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ComponentConfiguration
    {
        internal ComponentConfiguration(Type componentType)
        {
            this.Automations = new List<AutomationConfiguration>();
            this.ComponentType = componentType;
        }

        public void Apply(IComponentSchema schema)
        {
            foreach (var automation in Automations)
            {
                automation.Apply(schema);
            }
        }

        public IList<AutomationConfiguration> Automations { get; private set; }

        public Type ComponentType { get; private set; }
    }
}