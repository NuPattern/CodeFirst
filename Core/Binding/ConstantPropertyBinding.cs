namespace NuPattern.Binding
{
    using System;
    using System.Linq;

    public class ConstantPropertyBinding : PropertyBinding
    {
        private Type instanceType;
        private string propertyName;
        private object value;

        public ConstantPropertyBinding(Type instanceType, string propertyName, object value)
        {
            this.instanceType = instanceType;
            this.propertyName = propertyName;
            this.value = value;
        }

        public override void Refresh(object instance)
        {
            instanceType.GetProperty(propertyName).SetValue(instance, value, null);
        }

        public override string ToString()
        {
            var valueString = value.ToString();
            if (value is string)
                valueString = "\"" + valueString + "\"";

            return propertyName + "=" + valueString;
        }
    }
}