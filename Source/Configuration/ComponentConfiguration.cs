namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ComponentConfiguration : IVisitableConfiguration
    {
        internal ComponentConfiguration(Type componentType)
        {
            this.Automations = new List<AutomationConfiguration>();
            this.Properties = new List<PropertyConfiguration>();
            this.ComponentType = componentType;
        }

        public IList<AutomationConfiguration> Automations { get; private set; }

        public IList<PropertyConfiguration> Properties { get; private set; }

        public Type ComponentType { get; private set; }

        public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IConfigurationVisitor
        {
            foreach (var automation in Automations)
            {
                automation.Accept(visitor);
            }

            foreach (var property in Properties)
            {
                property.Accept(visitor);
            }

            return visitor;
        }
    }
}