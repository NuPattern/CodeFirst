namespace NuPattern.Schema
{
    using System;

    internal class ProductSchema : ContainerSchema, IProductSchema
    {
        /// <summary>
        /// Internal constructor used by tests to allow for easy 
        /// functional construction, by allowing you to create 
        /// the ToolkitSchema first and the product right in 
        /// the initializer.
        /// </summary>
        internal ProductSchema(string schemaId)
            : base(schemaId, null)
        {
        }
        
        public ProductSchema(string schemaId, ToolkitSchema toolkitSchema)
            : base(schemaId)
        {
            this.ToolkitSchema = toolkitSchema;
        }

        public ToolkitSchema ToolkitSchema { get; internal set; }

        IToolkitSchema IProductSchema.ToolkitSchema { get { return ToolkitSchema; } }
    }
}