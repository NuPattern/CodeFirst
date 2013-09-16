namespace NuPattern.Configuration.Schema
{
    using System;

    public interface IProductSchema : IContainerSchema
    {
        IToolkitSchema Toolkit { get; }
    }
}