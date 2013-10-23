namespace NuPattern.Automation
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reactive;
    using NuPattern.Properties;
    using NetFx.StringlyTyped;
    using NuPattern.Configuration;

    public class EventAutomationSettings : IAutomationSettings
    {
        private object annotations;

        public EventAutomationSettings(BindingConfiguration binding,
            ICommandAutomationSettings commandSettings)
        {
            Guard.NotNull(() => binding, binding);

            // TODO: eventually, either command or wizard may be specified.
            Guard.NotNull(() => commandSettings, commandSettings);

            if (!(typeof(IObservable<IEventPattern<object, EventArgs>>).IsAssignableFrom(binding.Type)))
            {
                throw new ArgumentException(Strings.EventAutomationSettings.EventTypeMustBeObservable(
                    Stringly.ToTypeName(typeof(IObservable<IEventPattern<object, EventArgs>>))));
            }

            this.Binding = binding;
            this.CommandSettings = commandSettings;
        }

        public BindingConfiguration Binding { get; private set; }

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