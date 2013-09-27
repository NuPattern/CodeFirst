namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class PropertyBindingConfiguration
    {
        public PropertyBindingConfiguration(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName { get; private set; }

        public object Value { get; set; }
        public BindingConfiguration ValueProvider { get; set; }
    }
}