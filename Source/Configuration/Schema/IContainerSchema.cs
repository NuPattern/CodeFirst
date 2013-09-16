namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;

    public interface IContainerSchema : IComponentSchema
    {
        IEnumerable<IComponentSchema> Components { get; }
        //IEnumerable<IExtensionPointSchema> Extensions { get; }

        IElementSchema CreateElementSchema(string schemaId);
        ICollectionSchema CreateCollectionSchema(string schemaId);
        // IExtensionPointSchema CreateExtensionPointSchema(string id);
    }
}