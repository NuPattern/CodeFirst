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
            get { return product.Get(() => Toolkit); }
            set { product.Set(() => Toolkit, value); }
        }

        public string Version
        {
            get { return product.Get(() => Version); }
            set { product.Set(() => Version, value); }
        }
    }
}