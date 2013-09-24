namespace NuPattern.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;

    public class EventAutomation : IAutomation, IDisposable
    {
        private EventAutomationSettings settings;
        private ICommandAutomation command;
        private IDisposable subscription;
        private IComponentContext scope;
        private object annotations;

        public EventAutomation(IComponentContext context, EventAutomationSettings settings)
        {
            this.settings = settings;
            this.scope = context.BeginScope(builder =>
                {
                    builder.RegisterType(settings.EventType);
                    if (settings.EventSettings != null)
                        builder.RegisterInstance(settings.EventSettings);
                });

            // EventAutomationSettings validates the cast is valid.
            var observable = (IObservable<IEventPattern<object, EventArgs>>)scope.Resolve(settings.EventType);
            subscription = observable.Subscribe(OnEvent);

            if (settings.CommandSettings != null)
                command = settings.CommandSettings.CreateAutomation(context);
        }

        public void Dispose()
        {
            scope.Dispose();
            subscription.Dispose();
            var disposable = command as IDisposable;
            if (disposable != null)
                disposable.Dispose();
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

        private void OnEvent(IEventPattern<object, EventArgs> @event)
        {
            if (command != null)
                command.Execute();
        }
    }
}