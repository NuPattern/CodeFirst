namespace NuPattern
{
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

    public class ToolkitBuilderFixture
    {
        [Fact]
        public void when_reusing_command_then_can_configure_multiple_events()
        {
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

            var product = InstantiateProduct(builder);

            var amazon = product.As<IAmazonWebServices>();
            amazon.AccessKey = "foo";
            amazon.SecretKey = "bar";

            Assert.Equal(2, TestCommand.ExecutedCount);
        }

        [Fact]
        public void when_reusing_lambda_command_then_can_configure_multiple_events()
        {
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

            var product = InstantiateProduct(builder);

            var amazon = product.As<IAmazonWebServices>();
            amazon.AccessKey = "foo";
            amazon.SecretKey = "bar";

            Assert.Equal(2, changes);
        }

        [Fact]
        public void when_building_toolkit_then_can_specify_event_automation()
        {
            var builder = new ToolkitBuilder("Simple", "1.0");
            var changes = new List<string>();

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.AccessKey)
                // TODO: Wizard
                .Execute()
                .TraceMessage("Access Key Changed");

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.AccessKey)
                .Execute(aws => changes.Add(aws.AccessKey));

            var product = InstantiateProduct(builder);

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
        private static Product InstantiateProduct(ToolkitBuilder builder)
        {
            // Runtime would call this builder when solution/project is opened:
            var schema = builder.Build();
            // Fake global resolve context.
            var rootContext = new ComponentContext();

            // User instantiates a product via Solution Builder:
            var product = new Product("MyWebService", typeof(IAmazonWebServices).FullName);
            ComponentMapper.SyncProduct(product, schema.Products.First());

            var productContext = rootContext.BeginScope(b => b.RegisterInstance(product));

            foreach (var setting in schema.Products.First().Automations)
            {
                product.AddAutomation(setting.CreateAutomation(productContext));
            }

            return product;
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
            public TestCommandSettings(string message)
            {
                this.Message = message;
            }

            public string Message { get; private set; }
        }
    }
}