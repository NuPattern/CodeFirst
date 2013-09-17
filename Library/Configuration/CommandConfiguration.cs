namespace NuPattern.Configuration
{
    using NuPattern.Automation;
    using NuPattern.Configuration.Schema;
    using NuPattern.Schema;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class CommandConfiguration : ICommandConfiguration
    {
        private Lazy<ICommandAutomationSettings> settings;

        public CommandConfiguration()
        {
            this.settings = new Lazy<ICommandAutomationSettings>(() => new CommandAutomationSettings(CommandType, CommandSettings));
        }

        [Required]
        public Type CommandType { get; set; }

        public object CommandSettings { get; set; }

        public ICommandAutomationSettings CreateSettings()
        {
            return settings.Value;
        }
    }
}