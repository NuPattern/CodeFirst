namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;

    internal class Product : Container, IProduct
    {
        private JObject product;

        public Product(JObject product)
            : base(product)
        {
            this.product = product;
            product.SetModel(this);

            var toolkit = product.Property("Toolkit");
            if (toolkit == null)
            {
                toolkit = new JProperty("Toolkit", new JObject());
                product.Add(toolkit);
            }

            this.Toolkit = new ToolkitInfo((JObject)toolkit.Value);
        }

        public IToolkitInfo Toolkit { get; private set; }
    }
}