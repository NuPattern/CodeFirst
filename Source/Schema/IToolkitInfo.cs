namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;

    public interface IToolkitInfo
    {
        string Id { get; }
        SemanticVersion Version { get; }
        IEnumerable<IProductInfo> Products { get; }
    }
}