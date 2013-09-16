namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;

    public interface IContainerInfo : IComponentInfo
    {
        IEnumerable<IComponentInfo> Components { get; }
    }
}