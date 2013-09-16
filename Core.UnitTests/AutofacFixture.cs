namespace NuPattern
{
    using Autofac;
    using System;
    using System.Linq;
    using Xunit;

    public class AutofacFixture
    {
        [Fact]
        public void when_disposing_lifetime_scope_then_disposes_created_component_and_registered_settings()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new Global());

            var container = builder.Build();

            var settings = new FooSettings();

            var scope = container.BeginLifetimeScope(b =>
            { 
                b.RegisterInstance(settings); 
                b.RegisterType(typeof(Foo)); 
            });

            var foo = scope.Resolve<Foo>();

            scope.Dispose();

            Assert.True(foo.IsDisposed);
            Assert.True(settings.IsDisposed);
        }

        public class Global { }

        public class FooSettings : IDisposable
        {
            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                this.IsDisposed = true;
            }
        }

        public class Foo : IDisposable
        {
            public Foo(Global global, FooSettings settings)
            {
            }

            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                this.IsDisposed = true;
            }
        }
    }
}