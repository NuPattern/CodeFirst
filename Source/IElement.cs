namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    public interface IElement : IContainer
    {
        new IElementInfo Schema { get; }

        new IElement Set<T>(string propertyName, T value);
    }
}