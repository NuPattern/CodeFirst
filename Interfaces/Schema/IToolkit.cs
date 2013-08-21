namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;

    public interface IToolkit
    {
        string Id { get; }
        string Version { get; }
        IEnumerable<IPatternSchema> Patterns { get; }
    }
}