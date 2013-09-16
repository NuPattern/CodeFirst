namespace NuPattern
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public interface IComponentContext : IDisposable
    {
        object Resolve(Type type);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IComponentContext BeginScope(Action<IComponentContextBuilder> configurationAction);
    }

    public interface IComponentContextBuilder
    {
        void RegisterType(Type type);
        void RegisterInstance(object instance);
    }
}