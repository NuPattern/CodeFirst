namespace NuPattern.Configuration
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class PatternConfiguration<TPattern> : ComponentConfiguration<TPattern>
    {
        private PatternConfiguration configuration;

        public PatternConfiguration()
            : this(new PatternConfiguration(typeof(TPattern)))
        {
        }

        internal PatternConfiguration(PatternConfiguration configuration)
            : base(configuration)
        {
            this.configuration = configuration;
        }

        public PatternConfiguration<TPattern> HasName(string name)
        {
            this.configuration.Name = name;
            return this;
        }

        public PatternConfiguration<TPattern> HasDisplayName(string displayName)
        {
            this.configuration.DisplayName = displayName;
            return this;
        }

        public PatternConfiguration<TPattern> HasDescription(string description)
        {
            this.configuration.Description = description;
            return this;
        }

        internal PatternConfiguration Configuration { get { return this.configuration; } }
    }
}