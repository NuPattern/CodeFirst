namespace NuPattern.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandAutomation : ICommandAutomation, IDisposable
    {
        private IComponentContext scope;
        private ICommand command;
        private object annotations;

        public CommandAutomation(IComponentContext context, CommandAutomationSettings settings)
        {
            Guard.NotNull(() => context, context);
            Guard.NotNull(() => settings, settings);

            this.scope = context.BeginScope(builder =>
            {
                builder.RegisterType(settings.CommandType);
                if (settings.CommandSettings != null)
                    builder.RegisterInstance(settings.CommandSettings);
            });

            // CommandAutomationSettings validates the cast is valid.
            this.command = (ICommand)this.scope.Resolve(settings.CommandType);
        }

        public void Dispose()
        {
            scope.Dispose();
        }

        public void Execute()
        {
            // TODO: wrap tracing, try..catch, etc.
            command.Execute();
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