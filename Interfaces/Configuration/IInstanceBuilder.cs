namespace NuPattern.Configuration
{
    using System;

    public interface IInstanceBuilder
    {
        string Name { get; }

        IInstanceBuilder DisplayName(string value);
        IInstanceBuilder Description(string value);
        IInstanceBuilder Visible(bool isVisible);

        IInstanceBuilder Parent { get; }
        IPatternBuilder Root { get; }
    }
}