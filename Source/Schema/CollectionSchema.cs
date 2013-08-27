namespace NuPattern.Schema
{
    using System;

    internal class CollectionSchema : ContainerSchema, ICollectionSchema
    {
        public CollectionSchema(string name)
            : base(name)
        {
        }

        public string ItemSchema { get; set; }
    }
}