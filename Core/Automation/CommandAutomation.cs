namespace NuPattern.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandAutomation : ICommandAutomation, IDisposable
    {
        private IBinding<ICommand> command;
        private object annotations;

        public CommandAutomation(IComponentContext context, CommandAutomationSettings settings)
        {
            Guard.NotNull(() => context, context);
            Guard.NotNull(() => settings, settings);

            // CommandAutomationSettings validates the cast is valid.
            this.command = context.Resolve<IBindingFactory>().CreateBinding<ICommand>(context, settings.Binding);
        }

        public void Dispose()
        {
            command.Dispose();
        }

        public void Execute()
        {
            command.Refresh();

            // TODO: wrap tracing, try..catch, etc.
            command.Instance.Execute();
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