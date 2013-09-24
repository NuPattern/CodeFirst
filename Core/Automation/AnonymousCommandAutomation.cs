using NuPattern.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuPattern.Automation
{
    public class AnonymousCommandAutomationSettings<T> : ICommandAutomationSettings
        where T : class
    {
        private Action<T> command;
        private object annotations;

        public AnonymousCommandAutomationSettings(Action<T> command)
        {
            Guard.NotNull(() => command, command);

            this.command = command;
        }

        public ICommandAutomation CreateAutomation(IComponentContext context)
        {
            Guard.NotNull(() => context, context);

            return new AnonymousCommandAutomation(command, context);
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

        IAutomation IAutomationSettings.CreateAutomation(IComponentContext context)
        {
            return CreateAutomation(context);
        }

        private class AnonymousCommandAutomation : ICommandAutomation, IDisposable
        {
            private Action<T> command;
            private IComponentContext scope;
            private object annotations;

            public AnonymousCommandAutomation(Action<T> command, IComponentContext context)
            {
                this.command = command;
                this.scope = context.BeginScope(builder => builder.RegisterInstance(((IComponent)context.Resolve(typeof(IComponent))).As<T>()));
            }

            public void Execute()
            {
                command.Invoke((T)scope.Resolve(typeof(T)));
            }

            public void Dispose()
            {
                scope.Dispose();
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
}
