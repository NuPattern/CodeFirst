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
        public Binding(object instance, IEnumerable<PropertyBinding> properties)
        {
            this.Instance = instance;
            this.Properties = properties.ToArray();
        }

        public object Instance { get; private set; }

        public IEnumerable<PropertyBinding> Properties { get; private set; }

        public void Refresh()
        {
            foreach (var property in Properties)
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