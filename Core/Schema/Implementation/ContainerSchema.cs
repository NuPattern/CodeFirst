namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    public abstract class ContainerSchema : ComponentSchema, IContainerSchema, IContainerInfo
    {
        private List<ComponentSchema> components = new List<ComponentSchema>();

        public ContainerSchema(string schemaId)
            : base(schemaId)
        {
        }

        public IEnumerable<IComponentSchema> Components { get { return components; } }

        public IElementSchema CreateElementSchema(string schemaId)
        {
            Guard.NotNullOrEmpty(() => schemaId, schemaId);

            var schema = new ElementSchema(schemaId);
            components.Add(schema);
            return schema;
        }

        public ICollectionSchema CreateCollectionSchema(string schemaId)
        {
            Guard.NotNullOrEmpty(() => schemaId, schemaId);

            var schema = new CollectionSchema(schemaId);
            components.Add(schema);
            return schema;
        }

        IEnumerable<IComponentInfo> IContainerInfo.Components { get { return components; } }
    }
}