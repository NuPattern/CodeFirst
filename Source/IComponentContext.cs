namespace NuPattern
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public interface IComponentContext : IDisposable
    {
        object Instantiate(Type type);
        object Resolve(Type type);
        object ResolveOptional(Type type);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IComponentContext BeginScope(Action<IComponentContextBuilder> configurationAction);
    }
}