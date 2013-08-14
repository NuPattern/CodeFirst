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
        }

        public string Toolkit
        {
            get { return (string)product.GetValue("Toolkit"); }
            set { product.Property("Toolkit").Value = value; }
        }

        public string Version
        {
            get { return (string)product.GetValue("Version"); }
            set { product.Property("Version").Value = value; }
        }
    }
}