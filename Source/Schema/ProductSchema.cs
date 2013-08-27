namespace NuPattern.Schema
{
    using System;

    internal class ProductSchema : ContainerSchema, IProductSchema
    {
        public IToolkitSchema Toolkit { get; set; }
    }
}