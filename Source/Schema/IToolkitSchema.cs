namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public interface IToolkitSchema
    {
        string Id { get; }
        SemanticVersion Version { get; }
        IEnumerable<IProductSchema> ProductSchemas { get; }

        IProductSchema CreateProductSchema(string id);
    }
}