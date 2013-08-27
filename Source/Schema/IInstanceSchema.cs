namespace NuPattern.Schema
{
    using System;

    public interface IInstanceSchema
    {
        string Name { get; set; }
        string DisplayName { get; set; }
        string Description { get; set; }

        bool IsVisible { get; set; }

        IInstanceSchema Parent { get; }
        IProductSchema Root { get; }
    }
}