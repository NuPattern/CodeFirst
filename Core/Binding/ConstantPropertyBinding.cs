namespace NuPattern.Binding
{
    using NuPattern.Core.Properties;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ConstantPropertyBinding : PropertyBinding
    {
        private object value;

        public ConstantPropertyBinding(Type instanceType, string propertyName, object value)
            : base(instanceType, propertyName)
        {
            this.value = value;
        }

        public override void Refresh(object instance)
        {
            base.SetValue(instance, value);
        }

        public override string ToString()
        {
            var valueString = value.ToString();
            if (value is string)
                valueString = "\"" + valueString + "\"";

            return PropertyName + "=" + valueString;
        }
    }
}