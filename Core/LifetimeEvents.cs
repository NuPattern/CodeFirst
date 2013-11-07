namespace NuPattern
{
    using System;

    public class LifetimeEvents<T> : ILifetimeEvents<T>
    {
        private object sender;

        public event ValueEventHandler<T> Created = (sender, args) => { };
        public event ValueEventHandler<T> Deleting = (sender, args) => { };
        public event ValueEventHandler<T> Deleted = (sender, args) => { };
        public event ValueEventHandler<T> Instantiated = (sender, args) => { };
        public event ValueEventHandler<T> Loading = (sender, args) => { };
        public event ValueEventHandler<T> Loaded = (sender, args) => { };

        public event ValueEventHandler<PropertyChangeEventArgs<T>> PropertyChanging = (sender, args) => { };
        public event ValueEventHandler<PropertyChangeEventArgs<T>> PropertyChanged = (sender, args) => { };

        public LifetimeEvents(object sender)
        {
            this.sender = sender;
        }

        public void RaiseCreated(T value)
        {
            Created(sender, value);
        }

        public void RaiseDeleting(T value)
        {
            Deleting(sender, value);
        }

        public void RaiseDeleted(T value)
        {
            Deleted(sender, value);
        }

        public void RaiseInstantiated(T value)
        {
            Instantiated(sender, value);
        }

        public void RaiseLoading(T value)
        {
            Loading(sender, value);
        }

        public void RaiseLoaded(T value)
        {
            Loaded(sender, value);
        }

        public void RaisePropertyChanging(T value, string propertyName, object oldValue, object newValue)
        {
            PropertyChanging(sender, new PropertyChangeEventArgs<T>(value, propertyName, oldValue, newValue));
        }

        public void RaisePropertyChanged(T value, string propertyName, object oldValue, object newValue)
        {
            PropertyChanged(sender, new PropertyChangeEventArgs<T>(value, propertyName, oldValue, newValue));
        }

        public void Forward(ComponentEvents events)
        {
            throw new NotImplementedException();
        }
    }
}
