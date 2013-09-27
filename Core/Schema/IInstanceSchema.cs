namespace NuPattern.Schema
{
    using NuPattern.Configuration;
    using System;

    public interface IInstanceSchema : IVisitable
    {
        string DisplayName { get; }
        string Description { get; }

        bool IsVisible { get; }

        IInstanceSchema Parent { get; }
        IProductSchema Root { get; }
    }
}