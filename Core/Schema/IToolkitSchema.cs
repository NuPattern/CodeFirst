namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;

    public interface IToolkitSchema : IVisitableSchema
    {
        string Id { get; }
        SemanticVersion Version { get; }
        IEnumerable<IProductSchema> Products { get; }

        IProductSchema CreateProductSchema(string id);
    }
}