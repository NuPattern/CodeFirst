namespace NuPattern.Binding
{
    using NuPattern.Configuration;
    using System;
    using System.Linq;

    public interface IBindingFactory
    {
        IBinding<T> CreateBinding<T>(BindingConfiguration configuration);
    }
}