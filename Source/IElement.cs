namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    public interface IElement : IContainer
    {
        new IElementSchema Schema { get; }

        new IElement Set<T>(string propertyName, T value);
    }
}