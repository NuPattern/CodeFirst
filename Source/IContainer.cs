namespace NuPattern
{
    using System;
    using System.Collections.Generic;

    public interface IContainer : IComponent
    {
        IEnumerable<IComponent> Components { get; }

        ICollection CreateCollection(string name, string schemaId);

        IElement CreateElement(string name, string schemaId);
    }
}