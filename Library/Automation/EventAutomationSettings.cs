namespace NuPattern.Automation
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Reactive;
    using NuPattern.Properties;
    using NetFx.StringlyTyped;

    public class EventAutomationSettings : IAutomationSettings
    {
        public EventAutomationSettings(
            Type eventType,
            object eventSettings,
            ICommandAutomationSettings commandSettings /*, ConfigurableReference wizardReference */)
        {
            Guard.NotNull(() => eventType, eventType);

            // TODO: eventually, either command or wizard may be specified.
            Guard.NotNull(() => commandSettings, commandSettings);

            if (!(typeof(IObservable<IEventPattern<object, EventArgs>>).IsAssignableFrom(eventType)))
            {
                throw new ArgumentException(Strings.EventAutomationSettings.EventTypeMustBeObservable(
                    Stringly.ToTypeName(typeof(IObservable<IEventPattern<object, EventArgs>>))));
            }

            this.EventType = eventType;
            this.EventSettings = eventSettings;
            this.CommandSettings = commandSettings;
        }

        public Type EventType { get; private set; }
        public object EventSettings { get; private set; }

        public ICommandAutomationSettings CommandSettings { get; private set; }
        // TODO: public WizardAutomationSettings WizardSettings { get; private set; }

        public IAutomation CreateAutomation(IComponentContext context)
        {
            return new EventAutomation(context, this);
        }
    }
}