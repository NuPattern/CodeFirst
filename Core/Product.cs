namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    internal class Product : Container, IProduct
    {
        public Product(string name, string schemaId)
            : base(name, schemaId, null)
        {
            this.Toolkit = new ToolkitInfo();
        }

        public new IProductSchema Schema
        {
            get { return (IProductSchema)base.Schema; }
            set { base.Schema = value; }
        }

        public ToolkitInfo Toolkit { get; private set; }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitProduct(this);
            return visitor;
        }

        public new Product Set<T>(string propertyName, T value)
        {
            base.Set(propertyName, value);
            return this;
        }

        IToolkitInfo IProduct.Toolkit { get { return Toolkit; } }
        IProduct IProduct.Set<T>(string propertyName, T value)
        {
            return Set<T>(propertyName, value);
        }
    }
}