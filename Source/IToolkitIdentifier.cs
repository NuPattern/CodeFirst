namespace NuPattern
{
    using System;

    public interface IToolkitIdentifier
    {
        string Id { get; }
        SemanticVersion Version { get; }
    }
}