namespace NuPattern.Schema
{
    using System;

    public interface IPatternSchema : IContainerSchema
    {
        IToolkit Toolkit { get; }
    }
}