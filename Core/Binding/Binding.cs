namespace NuPattern.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Binding<T> : Binding, IBinding<T>
    {
        public Binding(T instance, IEnumerable<PropertyBinding> properties)
            : base(instance, properties)
        {
        }

        public new T Instance
        {
            get { return (T)base.Instance; }
        }
    }

    public class Binding : IBinding
    {
        private PropertyBinding[] allProperties;
        private PropertyBinding[] dynamicProperties;

        private bool initialized;

        public Binding(object instance, IEnumerable<PropertyBinding> properties)
        {
            this.Instance = instance;
            this.allProperties = properties.ToArray();
        }

        public object Instance { get; private set; }

        public IEnumerable<PropertyBinding> Properties { get { return allProperties; } }

        public void Refresh()
        {
            if (!initialized)
            {
                // First time we initialize, we refresh the constant properties. 
                // It's the only time constant properties are evaluated, since they 
                // can't change later.
                foreach (var constantProperty in allProperties.OfType<ConstantPropertyBinding>())
                {
                    constantProperty.Refresh(Instance);
                }

                // We separate dynamic properties, which are used for refreshing.
                dynamicProperties = allProperties.OfType<ProvidedPropertyBinding>().ToArray();
                initialized = true;
            }

            // Note: only dynamic properties are refreshed subsequently.
            foreach (var property in dynamicProperties)
            {
                property.Refresh(Instance);
            }
        }

        public override string ToString()
        {
            return Instance.ToString() + ": " + string.Join(", ", Properties.Select(x => x.ToString()));
        }
    }
}