namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

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

        public ProductStore Store { get; internal set; }

        public ToolkitInfo Toolkit { get; internal set; }

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

        public override string ToString()
        {
            return Name + " : " + Toolkit.Id + "." + SchemaId + " (" + Toolkit.Version + ")";
        }

        protected override void OnRenaming(string oldName, string newName)
        {
            base.OnRenaming(oldName, newName);

            if (Store != null)
                Store.ThrowIfDuplicateRename(oldName, newName);
        }

        IProductStore IProduct.Store { get { return Store; } }
        IToolkitInfo IProduct.Toolkit { get { return Toolkit; } }
        IProduct IProduct.Set<T>(string propertyName, T value)
        {
            return Set<T>(propertyName, value);
        }
    }
}