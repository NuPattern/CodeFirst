using NuPattern.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NuPattern.Automation
{
    public class AnonymousCommandAutomationSettings : ICommandAutomationSettings
    {
        private Delegate command;
        private Type argumentType;
        private object annotations;

        public AnonymousCommandAutomationSettings(Delegate command, Type argumentType)
        {
            Guard.NotNull(() => command, command);
            Guard.NotNull(() => argumentType, argumentType);

            this.command = command;
            this.argumentType = argumentType;
        }

        public ICommandAutomation CreateAutomation(IComponentContext context)
        {
            Guard.NotNull(() => context, context);

            return new AnonymousCommandAutomation(command, argumentType, context);
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
            // TODO: should pre-compile invocation, resolving on the context, etc.
            private static readonly MethodInfo AsMethod = typeof(IComponent).GetMethod("As");
            private Delegate command;
            private Type argumentType;
            private IComponentContext scope;
            private object annotations;

            public AnonymousCommandAutomation(Delegate command, Type argumentType, IComponentContext context)
            {
                this.command = command;
                this.argumentType = argumentType;
                this.scope = context.BeginScope(builder => builder.RegisterInstance(
                    AsMethod.MakeGenericMethod(argumentType).Invoke(context.Resolve(typeof(IComponent)), null)));
            }

            public void Execute()
            {
                command.DynamicInvoke(scope.Resolve(argumentType));
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
