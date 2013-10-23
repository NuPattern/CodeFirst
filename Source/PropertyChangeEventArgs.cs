namespace NuPattern
{
    using System;
    using System.Linq;

    /// <summary>
    /// Event arguments for a property changing or changed event.
    /// </summary>
    public class PropertyChangeEventArgs : EventArgs
    {
        public PropertyChangeEventArgs(string propertyName, object oldValue, object newValue)
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