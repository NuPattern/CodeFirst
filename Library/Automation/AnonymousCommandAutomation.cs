using NuPattern.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuPattern.Automation
{
    public class AnonymousCommandAutomationSettings<T> : ICommandAutomationSettings
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

        private class AnonymousCommandAutomation : ICommandAutomation
        {
            private Action<T> command;
            private IComponentContext context;

            public AnonymousCommandAutomation(Action<T> command, IComponentContext context)
            {
                this.command = command;
                this.context = context;
            }

            public void Execute()
            {
                command.Invoke((T)context.Resolve(typeof(T)));
            }
        }
    }
}
