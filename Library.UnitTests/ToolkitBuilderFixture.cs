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
    using System.Reflection;

    public class ToolkitBuilderFixture
    {
        [Fact]
        public void when_reusing_command_then_can_configure_multiple_events()
        {
            var context = CreateRootContext();
            var builder = new ToolkitBuilder("Simple", "1.0");
            var changes = new List<string>();

            var commandConfig = builder.Product<IAmazonWebServices>()
                .Command().Configuration;

            commandConfig.CommandType = typeof(TestCommand);
            commandConfig.CommandSettings = new TestCommandSettings("foo");

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.AccessKey)
                .Execute(commandConfig);

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.SecretKey)
                .Execute(commandConfig);

            TestCommand.ExecutedCount = 0;

            var catalog = context.Resolve<IToolkitCatalog>();
            catalog.Add(builder);

            var product = InstantiateProduct(context);

            var amazon = product.As<IAmazonWebServices>();
            amazon.AccessKey = "foo";
            amazon.SecretKey = "bar";

            Assert.Equal(2, TestCommand.ExecutedCount);
        }

        [Fact]
        public void when_reusing_lambda_command_then_can_configure_multiple_events()
        {
            var context = CreateRootContext();
            var builder = new ToolkitBuilder("Simple", "1.0");
            var changes = 0;

            var commandConfig = builder.Product<IAmazonWebServices>()
                .Command(aws => changes++);

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.AccessKey)
                .Execute(commandConfig);

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.SecretKey)
                .Execute(commandConfig);

            TestCommand.ExecutedCount = 0;

            var catalog = context.Resolve<IToolkitCatalog>();
            catalog.Add(builder);

            var product = InstantiateProduct(context);

            var amazon = product.As<IAmazonWebServices>();
            amazon.AccessKey = "foo";
            amazon.SecretKey = "bar";

            Assert.Equal(2, changes);
        }

        [Fact]
        public void when_building_toolkit_then_can_specify_event_automation()
        {
            var context = CreateRootContext();
            var builder = new ToolkitBuilder("Simple", "1.0");
            var changes = new List<string>();

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.AccessKey)
                // TODO: Wizard
                .Execute()
                .TraceMessage("Access Key Changed");

            // TODO: allow chaining on method calls.

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.AccessKey)
                .Execute(aws => changes.Add(aws.AccessKey));


            // Later in time, when the user wants to....

            var catalog = context.Resolve<IToolkitCatalog>();
            catalog.Add(builder);

            var product = InstantiateProduct(context);

            // User changes a property via property browser:

            var amazon = product.As<IAmazonWebServices>();

            amazon.AccessKey = "blah";

            product.Set("AccessKey", "asdf");

            Assert.Equal(2, changes.Count);
            Assert.Equal("blah", changes[0]);
            Assert.Equal("asdf", changes[1]);
        }

        [Fact]
        public void when_building_toolkit_then_command_automation_can_use_dynamic_values()
        {
            var context = CreateRootContext();
            var builder = new ToolkitBuilder("Simple", "1.0");
            var changes = new List<string>();

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.AccessKey)
                // TODO: Wizard
                .Execute()
                .Test()
                .With(s => s.Message, aws => "AccessKey: " + aws.AccessKey + ", SecretKey: " + aws.SecretKey)
                .With(s => s.Count, aws => 3);

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.AccessKey)
                .Execute(aws => changes.Add(aws.AccessKey));

            var catalog = context.Resolve<IToolkitCatalog>();
            catalog.Add(builder);

            var product = InstantiateProduct(context);

            // User changes a property via property browser:

            var amazon = product.As<IAmazonWebServices>();

            amazon.AccessKey = "blah";

            product.Set("AccessKey", "asdf");

            Assert.Equal(2, changes.Count);
            Assert.Equal("blah", changes[0]);
            Assert.Equal("asdf", changes[1]);
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

            var context = new ComponentContext(cb.Build());

            return context;
        }
    }

    public class TestCommand : ICommand
    {
        private static ThreadLocal<int> executedCount = new ThreadLocal<int>(() => 0);

        public TestCommand(TestCommandSettings settings)
        {
            this.Settings = settings;
        }

        public static int ExecutedCount
        {
            get { return executedCount.Value; }
            set { executedCount.Value = value; }
        }

        public TestCommandSettings Settings { get; private set; }

        public void Execute()
        {
            executedCount.Value++;
        }
    }

    public class TestCommandSettings
    {
        public TestCommandSettings()
        {
        }

        public TestCommandSettings(string message)
        {
            this.Message = message;
        }

        [Required(AllowEmptyStrings = false)]
        public string Message { get; set; }

        public int Count { get; set; }
    }

    public class CommandConfiguration<T, TSettings>
        where T : class
        where TSettings : new()
    {
        private CommandConfiguration commandConfiguration;

        public CommandConfiguration(CommandConfiguration commandConfiguration)
        {
            this.commandConfiguration = commandConfiguration;
        }

        public CommandConfiguration<T, TSettings> With<TProperty>(Expression<Func<TSettings, TProperty>> property, Func<T, TProperty> value)
        {
            if (commandConfiguration.CommandSettings == null)
                commandConfiguration.CommandSettings = new TSettings();

            //((PropertyInfo)((MemberExpression)property.Body).Member).SetValue(commandConfiguration.CommandSettings, value())

            // TODO: Setup binding
            return this;
        }
    }

    public static class TestCommandExtensions
    {
        public static CommandConfiguration<T, TestCommandSettings> Test<T>(this CommandFor<T> configuration)
            where T : class
        {
            configuration.Configuration.CommandType = typeof(TestCommand);
            return new CommandConfiguration<T, TestCommandSettings>(configuration.Configuration);
        }
    }
}