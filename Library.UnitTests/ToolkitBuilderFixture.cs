namespace NuPattern
{
    using CommonComposition;
    using NuPattern.Configuration;
    using NuPattern.Schema;
    using NuPattern.Tookit.Simple;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Collections.Generic;
    using System.Threading;
    using Autofac;
    using Autofac.Builder;
    using System.Reflection;
    using System.ComponentModel;
    using System.Diagnostics;
    using Moq;

    public class ToolkitBuilderFixture
    {
        [Fact]
        public void when_reusing_command_then_can_configure_multiple_events()
        {
            var context = CreateRootContext();
            var builder = new SimpleBuilder();
            var traces = new List<string>();

            var listener = new Mock<TraceListener>(MockBehavior.Strict);
            listener.Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(s => traces.Add(s));

            System.Diagnostics.Trace.Listeners.Add(listener.Object);

            var catalog = context.Resolve<IToolkitCatalog>();
            catalog.Add(builder);

            var product = InstantiateProduct(context);

            var amazon = product.As<IAmazonWebServices>();
            amazon.AccessKey = "foo";

            Assert.Equal(1, traces.Count);
            Assert.Equal("Product: MyWebService (AccessKey: foo, SecretKey: )", traces[0]);

            amazon.SecretKey = "bar";

            Assert.Equal(2, traces.Count);
            Assert.Equal("Product: MyWebService (AccessKey: foo, SecretKey: bar)", traces[1]);
        }

        [Fact]
        public void when_command_depends_on_event_arg_then_it_can_access_it()
        {
            var context = CreateRootContext();
            var builder = new ToolkitBuilder("Test", "1.0");
            PropertyChangeEventArgs args = null;

            builder.Product<IAmazonWebServices>()
                .OnEvent()
                .PropertyChanged("Name")
                .Execute(new BindingConfiguration(typeof(CommandWithEventArgs)));

            var catalog = context.Resolve<IToolkitCatalog>();
            catalog.Add(builder);

            var product = InstantiateProduct(context);

            Assert.Null(CommandWithEventArgs.Event);

            var amazon = product.As<IAmazonWebServices>();
            amazon.Name = "Amazon";

            Assert.NotNull(CommandWithEventArgs.Event);
            Assert.Equal("Name", CommandWithEventArgs.Event.EventArgs.PropertyName);
            Assert.Equal("MyWebService", CommandWithEventArgs.Event.EventArgs.OldValue);
            Assert.Equal("Amazon", CommandWithEventArgs.Event.EventArgs.NewValue);
        }

        /// <summary>
        /// Simulates runtime behavior.
        /// </summary>
        private static Product InstantiateProduct(IComponentContext context)
        {
            var schema = context.Resolve<IToolkitCatalog>().Toolkits.First().Products.First();

            // User instantiates a product via Solution Builder:
            var product = new Product("MyWebService", typeof(IAmazonWebServices).FullName);
            ComponentMapper.SyncProduct(product, schema);

            var productContext = context.BeginScope(b => b.RegisterInstance(product));
            foreach (var setting in schema.Automations)
            {
                product.AddAutomation(setting.CreateAutomation(productContext));
            }

            return product;
        }

        private static IComponentContext CreateRootContext()
        {
            var cb = new ContainerBuilder();

            cb.RegisterComponents(typeof(ComponentContext).Assembly);
            cb.RegisterType<BindingFactory>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var context = new ComponentContext(cb.Build());

            return context;
        }
    }

    public class CommandWithEventArgs : ICommand
    {
        public static IEventPattern<object, PropertyChangeEventArgs> Event { get; set; }

        public CommandWithEventArgs(IEventPattern<object, PropertyChangeEventArgs> e)
        {
            Event = e;
        }

        public void Execute()
        {
        }
    }

    public class TestCommand : ICommand
    {
        private static ThreadLocal<int> executedCount = new ThreadLocal<int>(() => 0);

        public static int ExecutedCount
        {
            get { return executedCount.Value; }
            set { executedCount.Value = value; }
        }

        [Required(AllowEmptyStrings = false)]
        public string Message { get; set; }

        [DefaultValue(10)]
        public int Count { get; set; }

        public void Execute()
        {
            executedCount.Value++;
        }
    }

    public static class TestCommandExtensions
    {
        public static BindingConfiguration Test(this CommandFor configuration, string message, int count = 10)
        {
            return new BindingConfiguration<TestCommand>()
                .Property(x => x.Message, message)
                .Property(x => x.Count, count)
                .Configuration;
        }
    }
}