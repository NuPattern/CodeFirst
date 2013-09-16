namespace NuPattern.Configuration.Schema
{
    using System;

    public interface IInstanceSchema
    {
        string DisplayName { get; }
        string Description { get; }

        bool IsVisible { get; }

        IInstanceSchema Parent { get; }
        IProductSchema Root { get; }
    }
}