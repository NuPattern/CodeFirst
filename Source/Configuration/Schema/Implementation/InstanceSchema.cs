namespace NuPattern.Configuration.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    internal abstract class InstanceSchema : IInstanceSchema
    {
        protected InstanceSchema()
        {
            this.IsVisible = true;
        }

        public string DisplayName { get; set; }
        
        public string Description { get; set; }

        public bool IsVisible { get; set; }

        public InstanceSchema Parent { get; set; }

        public ProductSchema Root
        {
            get
            {
                InstanceSchema current = this;
                ProductSchema product = this as ProductSchema;
                while (current != null && product == null)
                {
                    current = current.Parent;
                    product = current as ProductSchema;
                }

                return product;
            }
        }

        IInstanceSchema IInstanceSchema.Parent { get { return Parent; } }
        IProductSchema IInstanceSchema.Root { get { return Root; } }
    }
}