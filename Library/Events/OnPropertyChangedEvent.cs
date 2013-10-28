namespace NuPattern.Events
{
    using NuPattern.Configuration;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Linq;

    public class OnPropertyChangedEvent : IObservable<IEventPattern<object, EventArgs>>
    {
        private Lazy<IObservable<IEventPattern<object, EventArgs>>> eventSource;

        public OnPropertyChangedEvent(IComponent component)
        {
            // Setup event observer, filter according to settings.PropertyName
            eventSource = new Lazy<IObservable<IEventPattern<object,EventArgs>>>(() => Observable.FromEventPattern<PropertyChangeEventArgs>(
                handler => component.Events.PropertyChanged += handler,
                handler => component.Events.PropertyChanged -= handler)
                .Where(e => e.EventArgs.PropertyName == this.PropertyName));
        }

        [Required(AllowEmptyStrings = false)]
        public string PropertyName { get; set; }

        public IDisposable Subscribe(IObserver<IEventPattern<object, EventArgs>> observer)
        {
            return eventSource.Value.Subscribe(observer);
        }
   }
}