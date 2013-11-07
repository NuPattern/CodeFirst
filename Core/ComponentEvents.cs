namespace NuPattern
{
    using System;

    public class ComponentEvents : IComponentEvents
    {
        public event EventHandler Created = (sender, args) => { };
        public event EventHandler Deleting = (sender, args) => { };
        public event EventHandler Deleted = (sender, args) => { };
        public event EventHandler Instantiated = (sender, args) => { };
        public event EventHandler Loading = (sender, args) => { };
        public event EventHandler Loaded = (sender, args) => { };

        public event EventHandler<PropertyChangeEventArgs> PropertyChanged = (sender, args) => { };
        public event EventHandler<PropertyChangeEventArgs> PropertyChanging = (sender, args) => { };

        public ComponentEvents(object sender)
        {
            this.Sender = sender;
        }

        public object Sender { get; private set; }

        public void RaiseCreated()
        {
            Created(Sender, EventArgs.Empty);
        }

        public void RaiseDeleting()
        {
            Deleting(Sender, EventArgs.Empty);
        }

        public void RaiseDeleted()
        {
            Deleted(Sender, EventArgs.Empty);
        }

        public void RaiseInstantiated()
        {
            Instantiated(Sender, EventArgs.Empty);
        }

        public void RaiseLoading()
        {
            Loading(Sender, EventArgs.Empty);
        }

        public void RaiseLoaded()
        {
            Loaded(Sender, EventArgs.Empty);
        }

        public void RaisePropertyChanging(string propertyName, object oldValue, object newValue)
        {
            PropertyChanging(Sender, new PropertyChangeEventArgs(propertyName, oldValue, newValue));
        }

        public void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            PropertyChanged(Sender, new PropertyChangeEventArgs(propertyName, oldValue, newValue));
        }
    }
}
