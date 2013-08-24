namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;

    public class CollectionConfiguration<TCollection> : ComponentConfiguration<TCollection>
    {
        private CollectionConfiguration configuration;

        protected CollectionConfiguration()
            : this(new CollectionConfiguration(typeof(TCollection)))
        {
        }

        internal CollectionConfiguration(CollectionConfiguration configuration)
            : base(configuration)
        {
            this.configuration = configuration;
        }

        public CollectionConfiguration<TCollection> HasName(string name)
        {
            this.configuration.Name = name;
            return this;
        }

        public CollectionConfiguration<TCollection> HasDisplayName(string displayName)
        {
            this.configuration.DisplayName = displayName;
            return this;
        }

        public CollectionConfiguration<TCollection> HasDescription(string description)
        {
            this.configuration.Description = description;
            return this;
        }

        internal CollectionConfiguration Configuration { get { return this.configuration; } }
    }
}