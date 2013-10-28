namespace NuPattern
{
    using System;

    public interface ILifetimeEvents<T>
    {
        event ValueEventHandler<T> Created;
        event ValueEventHandler<T> Deleting;
        event ValueEventHandler<T> Deleted;
        event ValueEventHandler<T> Instantiated;
        event ValueEventHandler<T> Loading;
        event ValueEventHandler<T> Loaded;

        event ValueEventHandler<PropertyChangeEventArgs<T>> PropertyChanged;
        event ValueEventHandler<PropertyChangeEventArgs<T>> PropertyChanging;
    }
}