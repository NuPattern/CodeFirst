namespace NuPattern.Commands
{
    using NuPattern.Configuration;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class TraceMessageCommand : ICommand
    {
        private TraceMessageSettings settings;

        public TraceMessageCommand(TraceMessageSettings settings)
        {
            this.settings = settings;
        }

        public void Execute()
        {
            System.Diagnostics.Trace.WriteLine(settings.Message);
        }
    }

    public class TraceMessageSettings
    {
        public TraceMessageSettings(string message)
        {
            this.Message = message;
        }

        [Required(AllowEmptyStrings = false)]
        public string Message { get; set; }
    }
}