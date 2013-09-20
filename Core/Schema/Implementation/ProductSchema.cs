namespace NuPattern.Schema
{
    using System;

    internal class ProductSchema : ContainerSchema, IProductSchema, IProductInfo
    {
        /// <summary>
        /// Internal constructor used by tests to allow for easy 
        /// functional construction, by allowing you to create 
        /// the ToolkitSchema first and the product right in 
        /// the initializer.
        /// </summary>
        internal ProductSchema(string schemaId)
            : base(schemaId)
        {
        }
        
        public ToolkitSchema Toolkit { get; internal set; }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.Visit<IProductSchema>(this);

            return base.Accept(visitor);
        }

        IToolkitInfo IProductInfo.Toolkit { get { return Toolkit; } }

        IToolkitSchema IProductSchema.Toolkit { get { return Toolkit; } }
    }
}