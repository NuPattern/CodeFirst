namespace NuPattern.Automation
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class CommandAutomationSettings : ICommandAutomationSettings
    {
        public CommandAutomationSettings(Type commandType, object commandSettings)
        {
            Guard.NotNull(() => commandType, commandType);

            if (!typeof(ICommand).IsAssignableFrom(commandType))
                throw new ArgumentException();

            this.CommandType = commandType;
            this.CommandSettings = commandSettings;
        }

        public Type CommandType { get; private set; }
        public object CommandSettings { get; private set; }

        public CommandAutomation CreateAutomation(IComponentContext context)
        {
            return new CommandAutomation(context, this);
        }

        IAutomation IAutomationSettings.CreateAutomation(IComponentContext context)
        {
            return CreateAutomation(context);
        }

        ICommandAutomation ICommandAutomationSettings.CreateAutomation(IComponentContext context)
        {
            return CreateAutomation(context);
        }
    }
}