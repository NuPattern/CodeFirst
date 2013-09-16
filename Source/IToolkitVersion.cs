namespace NuPattern
{
    using System;

    public interface IToolkitVersion
    {
        string Id { get; }
        SemanticVersion Version { get; }
    }
}