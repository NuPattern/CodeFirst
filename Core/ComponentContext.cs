namespace NuPattern
{
    using Autofac;
    using System;
    using System.Linq;

    internal class ComponentContext : IComponentContext
    {
        private ILifetimeScope scope;

        public ComponentContext()
            : this(new Autofac.ContainerBuilder().Build())
        {
        }

        public ComponentContext(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public object Resolve(Type type)
        {
            return scope.Resolve(type);
        }

        public IComponentContext BeginScope(Action<IComponentContextBuilder> configurationAction)
        {
            return new ComponentContext(scope.BeginLifetimeScope(builder => configurationAction(new ComponentContextBuilder(builder))));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private class ComponentContextBuilder : IComponentContextBuilder
        {
            private ContainerBuilder builder;

            public ComponentContextBuilder(ContainerBuilder builder)
            {
                this.builder = builder;
            }

            public void RegisterType(Type type)
            {
                builder.RegisterType(type).AsImplementedInterfaces().AsSelf();
            }

            public void RegisterInstance(object instance)
            {
                builder.RegisterInstance(instance).AsImplementedInterfaces().AsSelf();
            }
        }
    }
}