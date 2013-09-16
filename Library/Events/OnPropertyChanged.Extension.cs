namespace NuPattern.Configuration
{
    using NuPattern.Events;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Linq;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OnPropertyChangedExtension
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
}