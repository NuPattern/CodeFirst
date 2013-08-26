﻿namespace NuPattern.Schema
{
    using System;

    public interface IPatternSchema : IContainerSchema
    {
        IToolkitSchema Toolkit { get; }
    }
}