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

        IAutomation IAutomationSettings.CreateAutomation(IComponentContext context)
        {
            return CreateAutomation(context);
        }

        private class AnonymousCommandAutomation : ICommandAutomation, IDisposable
        {
            private Action<T> command;
            private IComponentContext scope;

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
        }
    }
}
