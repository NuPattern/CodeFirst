namespace NuPattern.Binding
{
    using System;
    using System.Linq;

    public class ProvidedPropertyBinding : PropertyBinding
    {
        private IBinding<IValueProvider> provider;

        public ProvidedPropertyBinding(Type instanceType, string propertyName, IBinding<IValueProvider> providerBinding)
            : base(instanceType, propertyName)
        {
            this.provider = providerBinding;
        }

        public override void Refresh(object instance)
        {
            provider.Refresh();
            base.SetValue(instance, provider.Instance.GetValue());
        }

        public override string ToString()
        {
            return this.PropertyName + "={" + provider.ToString() + "}";
        }
    }
}