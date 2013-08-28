﻿namespace NuPattern
{
    using System;

    public interface IToolkitInfo
    {
        string Id { get; }
        SemanticVersion Version { get; }
    }
}