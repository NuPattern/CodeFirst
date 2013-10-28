namespace NuPattern
{
    using Autofac;
    using System;
    using System.Linq;

    public class ComponentContext : IComponentContext
    {
        public ComponentContext()
            : this(new Autofac.ContainerBuilder().Build())
        {
        }

        public ComponentContext(ILifetimeScope scope)
        {
            this.Scope = scope;

            var builder = new ContainerBuilder();
            builder.RegisterInstance<IComponentContext>(this);
            builder.Update(scope.ComponentRegistry);
        }

        public ILifetimeScope Scope { get; private set; }

        public object Instantiate(Type type)
        {
            // Registers on demand and later resolves.
            if (!Scope.IsRegistered(type))
            {
                var builder = new ContainerBuilder();
                builder.RegisterType(type);
                builder.Update(Scope.ComponentRegistry);
            }

            return Scope.Resolve(type);
        }

        public object Resolve(Type type)
        {
            return Scope.Resolve(type);
        }

        public object ResolveOptional(Type type)
        {
            return Scope.ResolveOptional(type);
        }

        public IComponentContext BeginScope(Action<IComponentContextBuilder> configurationAction)
        {
            return new ComponentContext(Scope.BeginLifetimeScope(builder => configurationAction(new ComponentContextBuilder(builder))));
        }

        public void Dispose()
        {
            Scope.Dispose();
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