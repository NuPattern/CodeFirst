namespace NuPattern.Automation
{
    using System;
    using System.Linq;
    using System.Reactive;

    public class EventAutomation : IAutomation, IDisposable
    {
        private EventAutomationSettings settings;
        private ICommandAutomation command;
        private IDisposable subscription;
        private IComponentContext scope;

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

        private void OnEvent(IEventPattern<object, EventArgs> @event)
        {
            if (command != null)
                command.Execute();
        }
    }
}