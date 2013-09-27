namespace NuPattern.Automation
{
    using CommonComposition;
    using NuPattern.Configuration;
    using System;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class CommandAutomationSettingsFactory : IAutomationSettingsFactory<ICommandConfiguration, ICommandAutomationSettings>
    {
        public ICommandAutomationSettings CreateSettings(ICommandConfiguration configuration)
        {
            var anonymousConfig = configuration as AnonymousCommandConfiguration;
            var concreteConfig = configuration as CommandConfiguration;
            if (anonymousConfig != null)
            {
                return new AnonymousCommandAutomationSettings(anonymousConfig.Command, anonymousConfig.ArgumentType);
            }
            else if (concreteConfig != null)
            {
                return new CommandAutomationSettings(concreteConfig.CommandType, concreteConfig.CommandSettings);
            }

            return null;
        }
    }
}