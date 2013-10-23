namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BindingConfiguration
    {
        public BindingConfiguration()
        {
            this.Properties = new List<PropertyBindingConfiguration>();
        }

        public BindingConfiguration(Type type)
            : this()
        {
            this.Type = type;
        }

        public Type Type { get; internal set; }

        public IList<PropertyBindingConfiguration> Properties { get; private set; }

        public BindingConfiguration Property(string propertyName, object value)
        {
            Properties.Add(new PropertyBindingConfiguration(propertyName) { Value = value });

            return this;
        }

        public BindingConfiguration Property(string propertyName, BindingConfiguration valueProvider)
        {
            Properties.Add(new PropertyBindingConfiguration(propertyName) { ValueProvider = valueProvider });

            return this;
        }
    }
}