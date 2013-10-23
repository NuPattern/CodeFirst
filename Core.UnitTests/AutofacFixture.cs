namespace NuPattern.AutofacFixture
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Xunit;

    public class ContainerFixture
    {
        [Fact]
        public void when_creating_non_registered_instance_then_can_add_registration_on_the_fly()
        {
            var container = new ContainerBuilder().Build();

            var builder = new ContainerBuilder();
            builder.RegisterType(typeof(Global)).AsSelf().AsImplementedInterfaces();
            builder.Update(container);

            Assert.True(container.IsRegistered(typeof(Global)));

            container.Resolve<Global>();
        }

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

        [Fact]
        public void when_disposing_lifetime_scope_then_disposes_created_component_on_that_scope()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Disposable>().AsSelf().InstancePerLifetimeScope();

            var container = builder.Build();

            var scope = container.BeginLifetimeScope();

            //var foo = container.Resolve<Disposable>();
            var foo1 = scope.Resolve<Disposable>();
            var foo2 = scope.Resolve<Disposable>();

            //Assert.Same(foo, foo1);
            Assert.Same(foo1, foo2);

            scope.Dispose();

            Assert.True(foo1.IsDisposed);
        }

        [Fact]
        public void when_registering_on_scope_as_singleton_then_reuses_instance_but_disposes_transient_components_on_that_scope()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Global>().AsSelf().SingleInstance();

            var container = builder.Build();

            var perRequest = container.BeginLifetimeScope(b =>
                {
                    b.RegisterType<Local>().AsSelf().SingleInstance();
                    b.RegisterType<Disposable>().AsSelf();
                });

            var g1 = perRequest.Resolve<Global>();
            var g2 = perRequest.Resolve<Global>();
            var l1 = perRequest.Resolve<Local>();
            var l2 = perRequest.Resolve<Local>();
            var d1 = perRequest.Resolve<Disposable>();

            Assert.Same(g1, g2);
            Assert.Same(l1, l2);

            perRequest.Dispose();

            Assert.True(l1.IsDisposed);
            Assert.True(d1.IsDisposed);
        }

        [Fact]
        public void when_resolving_from_scope_then_resolves_locally_registered_providers()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<GlobalWithProviders>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ProviderA>().AsSelf().AsImplementedInterfaces();

            var container = builder.Build();

            var perRequest = container.BeginLifetimeScope(b =>
            {
                b.RegisterType<ProviderB>().AsSelf().AsImplementedInterfaces();
                b.RegisterType<ProviderC>().AsSelf().AsImplementedInterfaces();
            });

            var g1 = container.Resolve<GlobalWithProviders>();

            Assert.Equal(1, g1.Providers.Count());

            var g2 = perRequest.Resolve<GlobalWithProviders>();

            Assert.Equal(3, g2.Providers.Count());
        }

        [Fact]
        public void when_resolving_with_parameter_then_can_omit_other_parameters()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType(typeof(Global));
            builder.RegisterType(typeof(BarWithParameter));

            var container = builder.Build();

            var bar = container.Resolve<BarWithParameter>(TypedParameter.From("Foo"));

            Assert.Equal("Foo", bar.Parameter);
        }

        [Fact]
        public void when_providing_registration_source_then_can_change_instance_on_the_fly()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();

            var source = new EventArgsSource();

            container.ComponentRegistry.AddRegistrationSource(source);

            source.Args = EventArgs.Empty;

            var args = container.Resolve<EventArgs>();

            Assert.NotNull(args);
            Assert.Same(EventArgs.Empty, args);

            source.Args = new PropertyChangeEventArgs("Name", "Foo", "Bar");

            args = container.Resolve<PropertyChangeEventArgs>();

            Assert.Same(source.Args, args);
            Assert.Same(args, container.Resolve<EventArgs>());            
        }

        public class EventArgsSource : IRegistrationSource
        {
            public EventArgs Args { get; set; }

            public bool IsAdapterForIndividualComponents
            {
                get { return false; }
            }

            public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, 
                Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
            {
                if (Args == null)
                    return Enumerable.Empty<IComponentRegistration>();

                var typed = service as TypedService;
                if (typed == null)
                    return Enumerable.Empty<IComponentRegistration>();

                if (typed.ServiceType.IsAssignableFrom(Args.GetType()))
                {
                    return new[]
                    {
                        RegistrationBuilder.CreateRegistration(RegistrationBuilder.ForDelegate(
                            typed.ServiceType, (c, p) => this.Args))
                    };
                }

                return Enumerable.Empty<IComponentRegistration>();
            }
        }

        public class BarWithParameter
        {
            public BarWithParameter(Global global, string parameter)
            {
                this.Parameter = parameter;
            }

            public string Parameter { get; private set; }
        }

        [Fact(Skip = "Just used to meassure raw performance for component-level instantiation of automations via the component context")]
        public void when_factory_from_settings_to_automation_registered_then_can_resolve_it()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Product).Assembly, typeof(IProduct).Assembly).AsSelf().AsImplementedInterfaces();

            builder.Register<Func<IComponent, IAutomationSettings, IAutomation>>(c =>
            {
                return (owner, settings) => new CommandAutomation(owner, (ICommandAutomationSettings)settings);
            }).Keyed<Func<IComponent, IAutomationSettings, IAutomation>>(typeof(ICommandAutomationSettings));

            var container = builder.Build();

            var watch = Stopwatch.StartNew();

            var componentCount = 10000;
            var automationCount = 100;

            for (int i = 0; i < componentCount; i++)
            {
                var component = (IComponent)new Component();
                var componentScope = container.BeginLifetimeScope(cb =>
                    cb.RegisterInstance(component).AsSelf().AsImplementedInterfaces());

                for (int j = 0; j < automationCount; j++)
                {
                    var settings = (IAutomationSettings)new CommandAutomationSettings();
                    //var automationScope = componentScope.BeginLifetimeScope(cb =>
                    //    cb.RegisterInstance(settings).AsSelf().AsImplementedInterfaces());

                    foreach (var iface in settings.GetType().GetInterfaces())
                    {
                        var factory = componentScope.ResolveOptionalKeyed<Func<IComponent, IAutomationSettings, IAutomation>>(iface);
                        if (factory != null)
                        {
                            component.Automations.Add(factory.Invoke(component, settings));
                            break;
                        }
                    }
                }

                Assert.Equal(automationCount, component.Automations.Count);
            }

            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);

            //builder.register
        }

        public class Component : IComponent
        {
            public Component()
            {
                this.Automations = new List<IAutomation>();
            }

            public IList<IAutomation> Automations { get; private set; }
        }

        public interface IComponent
        {
            IList<IAutomation> Automations { get; }
        }

        public interface IAutomation { }
        public interface ICommandAutomation : IAutomation { }

        public interface IAutomationSettings { }
        public interface ICommandAutomationSettings : IAutomationSettings { }

        public class AutomationSettings : IAutomationSettings { }
        public class CommandAutomationSettings : AutomationSettings, ICommandAutomationSettings { }

        public class Automation : IAutomation { }

        public class CommandAutomation : Automation, ICommandAutomation
        {
            public CommandAutomation(IComponent owner, ICommandAutomationSettings settings)
            {
            }
        }

        public class GlobalWithProviders
        {
            public GlobalWithProviders(IEnumerable<IProvider> providers)
            {
                this.Providers = providers;
            }

            public IEnumerable<IProvider> Providers { get; private set; }
        }

        public interface IProvider { }
        public class ProviderA : IProvider { }
        public class ProviderB : IProvider { }
        public class ProviderC : IProvider { }

        public class Global : IDisposable
        {
            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                this.IsDisposed = true;
            }
        }

        public class Local : IDisposable
        {
            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                this.IsDisposed = true;
            }
        }

        public class Disposable : IDisposable
        {
            public Disposable(IComponentContext context)
            {
            }

            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                this.IsDisposed = true;
            }
        }

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