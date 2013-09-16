namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class ProductConfiguration<T> : ComponentConfiguration<T>
        where T : class
    {
        private ProductConfiguration configuration;

        // TODO: add public ctor so that it can be created standalone

        internal ProductConfiguration(ProductConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override ComponentConfiguration Configuration { get { return configuration; } }
    }
}