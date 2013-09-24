namespace NuPattern.Automation
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reactive;
    using NuPattern.Properties;
    using NetFx.StringlyTyped;

    public class EventAutomationSettings : IAutomationSettings
    {
        private object annotations;

        public EventAutomationSettings(
            Type eventType,
            object eventSettings,
            ICommandAutomationSettings commandSettings)
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
        // TODO: public IWizardAutomationSettings WizardSettings { get; private set; }

        public IAutomation CreateAutomation(IComponentContext context)
        {
            return new EventAutomation(context, this);
        }

        #region Annotations

        public void AddAnnotation(object annotation)
        {
            Annotator.AddAnnotation(ref annotations, annotation);
        }

        public object Annotation(Type type)
        {
            return Annotator.Annotation(annotations, type);
        }

        public IEnumerable<object> Annotations(Type type)
        {
            return Annotator.Annotations(annotations, type);
        }

        public void RemoveAnnotations(Type type)
        {
            Annotator.RemoveAnnotations(ref annotations, type);
        }

        #endregion
    }
}