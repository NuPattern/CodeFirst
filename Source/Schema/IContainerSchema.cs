namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;

    public interface IContainerSchema : IComponentSchema
    {
        IEnumerable<IComponentSchema> Components { get; }

        //IEnumerable<IExtensionPointSchema> Extensions { get; }
    }
}