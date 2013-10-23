namespace NuPattern
{
    using NuPattern.Configuration;
    using System;
    using System.Linq;

    public interface IBindingFactory
    {
        // TODO: need declarative InstancePerLifetimeScope for the factory in order to remove the context parameter.
        IBinding<T> CreateBinding<T>(IComponentContext context, BindingConfiguration configuration);
    }
}