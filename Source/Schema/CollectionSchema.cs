namespace NuPattern.Schema
{
    using System;

    internal class CollectionSchema : ContainerSchema, ICollectionSchema
    {
        public CollectionSchema(string name, ComponentSchema items)
            : base(name)
        {
            items.Parent = this;
            this.Items = items;
        }

        public IComponentSchema Items { get; set; }

        IComponentSchema ICollectionSchema.Items { get { return Items; } }
    }
}