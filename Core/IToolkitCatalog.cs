namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;

    public interface IToolkitCatalog
    {
        void Add(IToolkitBuilder builder);

        IEnumerable<IToolkitInfo> Toolkits { get; }

        IToolkitInfo Find(string toolkitId);
    }
}
