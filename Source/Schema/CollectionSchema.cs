namespace NuPattern.Schema
{
    using System;

    internal class CollectionSchema : ContainerSchema, ICollectionSchema
    {
        public string ItemSchema { get; set; }
    }
}