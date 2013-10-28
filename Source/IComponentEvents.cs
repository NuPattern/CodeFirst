namespace NuPattern
{
    using System;

    public interface IComponentEvents
    {
        event EventHandler Created;
        event EventHandler Deleting;
        event EventHandler Deleted;
        event EventHandler Instantiated;
        event EventHandler Loading;
        event EventHandler Loaded;

        event EventHandler<PropertyChangeEventArgs> PropertyChanged;
        event EventHandler<PropertyChangeEventArgs> PropertyChanging;
    }
}