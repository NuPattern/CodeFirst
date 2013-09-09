namespace NuPattern
{
    using System;
    using System.Linq;

    public class PropertyChangedEventArgs : EventArgs
    {
        public PropertyChangedEventArgs(string propertyName, object oldValue, object newValue)
        {
            Guard.NotNullOrEmpty(() => propertyName, propertyName);

            this.PropertyName = propertyName;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public string PropertyName { get; private set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }
    }
}