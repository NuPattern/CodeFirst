namespace NuPattern.Events
{
    using NuPattern.Configuration;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Linq;

    public static class OnPropertyChanged
    {
        public static EventConfiguration<T> PropertyChanged<T>(this EventFor<T> configuration, Expression<Func<T, object>> propertyExpression)
            where T : class
        {
            return PropertyChanged(configuration, ((MemberExpression)propertyExpression.Body).Member.Name);
        }

        public static EventConfiguration<T> PropertyChanged<T>(this EventFor<T> configuration, string propertyName)
            where T : class
        {
            var eventConfig = new EventConfiguration
            {
                EventType = typeof(OnPropertyChangedEvent),
                EventSettings = new OnPropertyChangedEventSettings { PropertyName = propertyName },
            };

            configuration.Configuration.Automations.Add(eventConfig);

            return new EventConfiguration<T>(configuration.Configuration, eventConfig);
        }
    }

    public class OnPropertyChangedEvent : IObservable<IEventPattern<object, EventArgs>>
    {
        private IObservable<IEventPattern<object, EventArgs>> eventSource;

        public OnPropertyChangedEvent(IComponent component, OnPropertyChangedEventSettings settings)
        {
            // Setup event observer, filter according to settings.PropertyName
            eventSource = Observable.FromEventPattern<PropertyChangedEventArgs>(
                handler => component.PropertyChanged += handler,
                handler => component.PropertyChanged -= handler)
                .Where(e => e.EventArgs.PropertyName == settings.PropertyName);
        }

        public IDisposable Subscribe(IObserver<IEventPattern<object, EventArgs>> observer)
        {
            return eventSource.Subscribe(observer);
        }
    }

    public class OnPropertyChangedEventSettings
    {
        public string PropertyName { get; set; }
    }
}