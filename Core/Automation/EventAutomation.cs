namespace NuPattern.Automation
{
    using Autofac.Builder;
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;

    public class EventAutomation : IAutomation, IDisposable, IRegistrationSource
    {
        private IComponentContext scope;
        private Lazy<ICommandAutomation> command;
        private IDisposable subscription;

        private IBinding<IObservable<IEventPattern<object, EventArgs>>> binding;
        private object annotations;

        private IEventPattern<object, EventArgs> lastEvent;

        public EventAutomation(IComponentContext context, EventAutomationSettings settings)
        {
            Guard.NotNull(() => context, context);
            Guard.NotNull(() => settings, settings);

            this.scope = context.BeginScope(c => { });
            // TODO: see how to make this generic and not tied to Autofac somehow?
            ((ComponentContext)scope).Scope.ComponentRegistry.AddRegistrationSource(this);

            this.binding = scope.Resolve<IBindingFactory>()
                .CreateBinding<IObservable<IEventPattern<object, EventArgs>>>(scope, settings.Binding);

            this.binding.Refresh();
            this.subscription = this.binding.Instance.Subscribe(OnEvent);

            if (settings.CommandSettings != null)
                command = new Lazy<ICommandAutomation>(() => settings.CommandSettings.CreateAutomation(scope));
        }

        public void Dispose()
        {
            subscription.Dispose();
            binding.Dispose();

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
            lastEvent = @event;

            if (command != null)
                command.Value.Execute();
        }

        bool IRegistrationSource.IsAdapterForIndividualComponents
        {
            get { return false; }
        }

        IEnumerable<IComponentRegistration> IRegistrationSource.RegistrationsFor(Service service,
            Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            var typed = service as TypedService;

            if (lastEvent != null && typed != null)
            {
                if (typed.ServiceType.IsAssignableFrom(lastEvent.GetType()))
                {
                    return new[]
                        {
                            RegistrationBuilder.CreateRegistration(RegistrationBuilder.ForDelegate(
                                typed.ServiceType, (c, p) => lastEvent))
                        };
                }
                else if (lastEvent.EventArgs != null &&
                    typed.ServiceType.IsAssignableFrom(lastEvent.EventArgs.GetType()))
                {
                    return new[]
                        {
                            RegistrationBuilder.CreateRegistration(RegistrationBuilder.ForDelegate(
                                typed.ServiceType, (c, p) => lastEvent))
                        };
                }
            }

            return Enumerable.Empty<IComponentRegistration>();
        }
    }
}