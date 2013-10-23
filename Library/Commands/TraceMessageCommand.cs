namespace NuPattern.Commands
{
    using NuPattern.Configuration;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class TraceMessageCommand : ICommand
    {
        public TraceMessageCommand(IProduct product)
        {
        }

        [Required(AllowEmptyStrings = false)]
        public string Message { get; set; }

        public void Execute()
        {
            System.Diagnostics.Trace.WriteLine(this.Message);
        }
    }
}