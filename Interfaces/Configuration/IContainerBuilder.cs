namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Generic;

    public interface IContainerBuilder : IComponentBuilder
    {
        IEnumerable<IComponentBuilder> Elements { get; }

        IElementBuilder Element(string name);

        ICollectionBuilder Collection(string name);

        //IEnumerable<IExtensionPointSchema> Extensions { get; }
    }
}