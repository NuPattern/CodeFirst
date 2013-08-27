namespace NuPattern.Schema
{
    using System;

    internal class ProductSchema : ContainerSchema, IProductSchema
    {
        public ProductSchema(string name)
            : base(name)
        {
        }

        public IToolkitSchema Toolkit { get; set; }
    }
}