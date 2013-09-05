namespace NuPattern
{
    using NuPattern.Schema;
    using System;

    public interface IToolkitBuilder
    {
        IToolkitSchema Build();
    }
}
