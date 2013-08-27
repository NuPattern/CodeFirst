namespace NuPattern.Schema
{
    using System;

    public interface IInstanceSchema
    {
        string Name { get; }
        string DisplayName { get; }
        string Description { get; }

        bool IsVisible { get; }

        IInstanceSchema Parent { get; }
        IProductSchema Root { get; }
    }
}