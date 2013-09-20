namespace NuPattern.Schema
{
    using System;

    public interface IInstanceSchema : IVisitableSchema
    {
        string DisplayName { get; }
        string Description { get; }

        bool IsVisible { get; }

        IInstanceSchema Parent { get; }
        IProductSchema Root { get; }
    }
}