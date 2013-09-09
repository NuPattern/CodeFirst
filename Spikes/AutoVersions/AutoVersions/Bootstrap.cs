namespace AutoVersions
{
    using Autofac;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;
    using Autofac.Builder;
    using Autofac.Core;
    using System.IO;
    using System.Reflection;
    using System.Collections;
    using Runtime;

    public class Bootstrap
    {
        [Fact]
        public void when_registering_enumerable_then_can_take_dependency()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<InfosAdapter>().As<IEnumerable<IInfo>>();
            builder.RegisterType<DependsOnInfo>();
            var container = builder.Build();

            Assert.Equal(2, container.Resolve<DependsOnInfo>().Infos.Count);

        }

        public class DependsOnInfo
        {
            public DependsOnInfo(IEnumerable<IInfo> infos)
            {
                this.Infos = infos.ToList();
            }

            public List<IInfo> Infos { get; set; }
        }

        public interface IInfo { }
        public class Info : IInfo { }

        public class InfosAdapter : IEnumerable<IInfo>
        {
            private IEnumerable<IInfo> infos;

            public InfosAdapter()
            {
                this.infos = new IInfo[] { new Info(), new Info() };
            }

            public IEnumerator<IInfo> GetEnumerator()
            {
                return infos.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Fact]
        public void when_resolving_lifetimescope_then_succeeds()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();

            var scope = container.Resolve<ILifetimeScope>();

            Assert.NotNull(scope);
        }

        [Fact]
        public void when_component_registered_on_builder_then_can_update_lifetime_scope()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();

            var childBuilder = new ContainerBuilder();
            childBuilder.RegisterType<Foo>();

            using (var scope = container.BeginLifetimeScope())
            {
                childBuilder.Update(scope.ComponentRegistry);

                var component = scope.Resolve<Foo>();
                Assert.NotNull(component);
            }
        }

        [Fact]
        public void when_disposing_child_scope_then_does_not_dispose_registered_external_instances()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            var component = new Foo();

            using (var scope = container.BeginLifetimeScope(b => b.Register(c => component)))
            {
            }

            Assert.False(component.IsDisposed);
        }

        public class Foo : IDisposable
        {
            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                this.IsDisposed = true;
            }
        }

        [Fact]
        public void when_nesting_containers_then_gets_service_from_closest()
        {
            var builder = new ContainerBuilder();

            builder.Register<IPatternElement>(c => new PatternElement()).SingleInstance();

            var container = builder.Build();

            var scopes = new List<ILifetimeScope>();

            AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = string.Join(";", Directory.EnumerateDirectories("."));
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    // Custom-resolve assemblies deployed with the runtime.
                    if (args.Name.StartsWith("Runtime"))
                    {
                        var asmFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll");
                        if (File.Exists(asmFile))
                        {
                            Console.WriteLine("Resolved runtime assembly from LoadFrom context: " + args.Name);
                            return Assembly.LoadFrom(asmFile);
                        }
                    }

                    return null;
                };

            foreach (var directory in Directory.EnumerateDirectories(".").Reverse())
            {
                var dir = directory;
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    // Only resolve non-runtime assemblies
                    if (!args.Name.StartsWith("Runtime"))
                    {
                        var asmFile = Path.Combine(directory, args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll");
                        if (File.Exists(asmFile))
                        {
                            var name = AssemblyName.GetAssemblyName(asmFile);
                            if (name.FullName == args.Name)
                                return Assembly.LoadFile(new FileInfo(asmFile).FullName);
                        }
                    }

                    return null;
                };

                var assemblies = new List<Assembly>();
                foreach (var file in Directory.EnumerateFiles(dir, "*.dll"))
                {
                    assemblies.Add(Assembly.LoadFile(new FileInfo(file).FullName));
                }

                scopes.Add(container.BeginLifetimeScope(b =>
                    b.RegisterAssemblyTypes(assemblies
                        .Where(asm => !asm.ManifestModule.FullyQualifiedName.StartsWith("Autofac"))
                        .ToArray())
                        //.WithParameter(
                        //    (p, c) => p.GetCustomAttribute<KeyAttribute>() != null,
                        //    (p, c) =>
                        //    {
                        //        var attr = p.GetCustomAttribute<KeyAttribute>();
                        //        object value;
                        //        c.TryResolveKeyed(attr.Key, p.ParameterType, out value);
                        //        return value;
                        //    })
                        //.As(t =>
                        //    {
                        //        var key = t.GetCustomAttribute<KeyAttribute>();
                        //        if (key == null)
                        //            return t.GetInterfaces().Select(i => new TypedService(i));
                        //        else
                        //            return t.GetInterfaces().Select(i => new KeyedService(key.Key, i));
                        //    })));
                    .AsSelf()
                    .AsImplementedInterfaces()));
            }

            Assert.Equal(2, scopes.Count);

            Console.WriteLine();

            Console.WriteLine("Instantiating old toolkit component, which references Runtime v1 and Library v1");
            Console.WriteLine(scopes[0].Resolve<ICloneable>().Clone().ToString());
            Console.WriteLine("Instantiating new toolkit component, which references Runtime v2 and Library v2");
            Console.WriteLine(scopes[1].Resolve<ICloneable>().Clone().ToString());
        }
    }
}
