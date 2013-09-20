namespace NuPattern.AutofacFixture
{
    using Autofac;
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
        public void when_resolving_with_parameter_then_can_omit_other_parameters()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType(typeof(Global));
            builder.RegisterType(typeof(BarWithParameter));

            var container = builder.Build();

            var bar = container.Resolve<BarWithParameter>(TypedParameter.From("Foo"));

            Assert.Equal("Foo", bar.Parameter);
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