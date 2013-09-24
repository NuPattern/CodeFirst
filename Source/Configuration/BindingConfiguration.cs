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

        public Type Type { get; set; }

        public IList<PropertyBindingConfiguration> Properties { get; set; }
    }
}