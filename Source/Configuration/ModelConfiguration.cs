namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ModelConfiguration
    {
        private Dictionary<Type, ProductConfiguration> products = new Dictionary<Type, ProductConfiguration>();

        public IEnumerable<Type> ConfiguredProducts { get { return products.Keys; } }

        public ProductConfiguration Product(Type productType)
        {
            return products.GetOrAdd(productType, type => new ProductConfiguration(type));
        }
    }
}