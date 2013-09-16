namespace NuPattern.Configuration
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class ElementConfiguration<TElement> : ComponentConfiguration<TElement>
    {
        private ElementConfiguration configuration;

        protected ElementConfiguration()
            : this(new ElementConfiguration(typeof(TElement)))
        {
        }

        internal ElementConfiguration(ElementConfiguration configuration)
            : base(configuration)
        {
            this.configuration = configuration;
        }

        public ElementConfiguration<TElement> HasName(string name)
        {
            this.configuration.Name = name;
            return this;
        }

        public ElementConfiguration<TElement> HasDisplayName(string displayName)
        {
            this.configuration.DisplayName = displayName;
            return this;
        }

        public ElementConfiguration<TElement> HasDescription(string description)
        {
            this.configuration.Description = description;
            return this;
        }

        internal ElementConfiguration Configuration { get { return this.configuration; } }
    }
}