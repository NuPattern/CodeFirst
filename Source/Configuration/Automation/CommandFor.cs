namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class CommandFor<T> where T : class
    {
        internal CommandFor(CommandConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public CommandConfiguration Configuration { get; private set; }
    }
}