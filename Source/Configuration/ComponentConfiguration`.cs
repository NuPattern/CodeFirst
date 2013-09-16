namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public abstract class ComponentConfiguration<T> where T : class
    {
        public abstract ComponentConfiguration Configuration { get; }
    }
}