namespace NuPattern
{
    using System;

    public interface IComponentContextBuilder
    {
        void RegisterType(Type type);
        void RegisterInstance(object instance);
    }
}