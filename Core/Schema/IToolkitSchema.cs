namespace NuPattern.Schema
{
    using NuPattern.Configuration;
    using System;
    using System.Collections.Generic;

    public interface IToolkitSchema
    {
        string Id { get; }
        SemanticVersion Version { get; }
        IEnumerable<IProductSchema> Products { get; }

        IProductSchema CreateProductSchema(string id);

        bool Accept(ISchemaVisitor visitor);
    }
}