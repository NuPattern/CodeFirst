namespace NuPattern
{
    using System;

    public class PropertyChangeEventArgs<T> : PropertyChangeEventArgs
    {
        public PropertyChangeEventArgs(T instance, string propertyName, object oldValue, object newValue)
            : base(propertyName, oldValue, newValue)
        {
            Guard.NotNull(() => instance, instance);

            this.Instance = instance;
        }

        public T Instance { get; private set; }
    }
}
