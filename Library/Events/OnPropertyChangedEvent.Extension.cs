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
    public static class OnPropertyChangedEventExtension
    {
        public static EventConfiguration<T> PropertyChanged<T>(this EventFor<T> configuration, Expression<Func<T, object>> propertyExpression)
            where T : class
        {
            return PropertyChanged(configuration, Reflect<T>.GetPropertyName(propertyExpression));
        }

        public static EventConfiguration<T> PropertyChanged<T>(this EventFor<T> configuration, string propertyName)
            where T : class
        {
            var eventConfig = new EventConfiguration
            {
                EventBinding = new BindingConfiguration<OnPropertyChangedEvent>()
                    .Property(x => x.PropertyName, propertyName)
                    .Configuration
            };

            configuration.Configuration.Automations.Add(eventConfig);

            return new EventConfiguration<T>(configuration.Configuration, eventConfig);
        }
    }
}