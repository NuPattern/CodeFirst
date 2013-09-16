namespace NuPattern.Configuration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class CommandConfiguration
    {
        public CommandConfiguration()
        {
        }

        public CommandConfiguration(Type commandType, object commandSettings)
        {
            this.CommandType = commandType;
            this.CommandSettings = commandSettings;
        }

        [Required]
        public Type CommandType { get; set; }

        public object CommandSettings { get; set; }
    }
}