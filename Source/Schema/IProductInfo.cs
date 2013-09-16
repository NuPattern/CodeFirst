namespace NuPattern.Schema
{
    using System;

    public interface IProductInfo : IContainerInfo
    {
        IToolkitInfo Toolkit { get; }
    }
}