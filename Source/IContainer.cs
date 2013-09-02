namespace NuPattern
{
    using System;
    using System.Collections.Generic;

    public interface IContainer : IComponent
    {
        IEnumerable<IComponent> Components { get; }

        IEnumerable<IProduct> Extensions { get; }

        ICollection CreateCollection(string name);

        ICollection CreateCollection(string name, string definition);

        IElement CreateElement(string name, string definition);
    }
}