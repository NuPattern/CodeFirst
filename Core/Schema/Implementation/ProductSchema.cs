namespace NuPattern.Schema
{
    using System;

    public class ProductSchema : ContainerSchema, IProductSchema, IProductInfo
    {
        private ToolkitSchema toolkit;

        public ProductSchema(string schemaId, ToolkitSchema toolkit)
            : base(schemaId)
        {
            this.toolkit = toolkit;
        }

        public IToolkitSchema Toolkit { get { return toolkit; } }

        public override bool Accept(ISchemaVisitor visitor)
        {
            if (visitor.VisitEnter(this))
            {
                foreach (var property in Properties)
                {
                    if (!property.Accept(visitor))
                        break;
                }

                foreach (var component in Components)
                {
                    if (!component.Accept(visitor))
                        break;
                }
            }

            return visitor.VisitLeave(this);
        }

        IToolkitInfo IProductInfo.Toolkit { get { return toolkit; } }
    }
}