namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class CommandFor
    {
        internal CommandFor(BindingConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public BindingConfiguration Configuration { get; private set; }
    }
}