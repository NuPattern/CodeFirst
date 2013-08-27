namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using NuPattern.Schema;
    using System;

    internal class Product : Container, IProduct
    {
        private JObject product;

        public Product(JObject product)
            : this(product, null)
        {
        }

        public Product(JObject product, JProperty property)
            : base(product, property)
        {
            this.product = product;

            var toolkit = product.Property(Prop.Toolkit);
            if (toolkit == null)
            {
                toolkit = new JProperty(Prop.Toolkit, new JObject());
                product.Add(toolkit);
            }

            this.Toolkit = new ToolkitInfo((JObject)toolkit.Value);
        }

        public new IProductSchema Schema { get; set; }

        public IToolkitInfo Toolkit { get; private set; }

        public new IProduct Set<T>(string propertyName, T value)
        {
            base.Set(propertyName, value);
            return this;
        }
    }
}