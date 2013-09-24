namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class CommandConfiguration : ICommandConfiguration
    {
        [Required]
        public Type CommandType { get; set; }

        public object CommandSettings { get; set; }
    }
}