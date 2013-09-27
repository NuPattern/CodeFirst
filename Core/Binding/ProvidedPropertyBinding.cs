namespace NuPattern.Binding
{
    using System;
    using System.Linq;

    public class ProvidedPropertyBinding : PropertyBinding
    {
        private Type instanceType;
        private string propertyName;
        private IBinding<IValueProvider> provider;

        public ProvidedPropertyBinding(Type instanceType, string propertyName, IBinding<IValueProvider> providerBinding)
        {
            this.instanceType = instanceType;
            this.propertyName = propertyName;
            this.provider = providerBinding;
        }

        public override void Refresh(object instance)
        {
            provider.Refresh();
            instanceType.GetProperty(propertyName).SetValue(instance, provider.Instance.GetValue(), null);
        }

        public override string ToString()
        {
            return propertyName + "={" + provider.ToString() + "}";
        }
    }
}