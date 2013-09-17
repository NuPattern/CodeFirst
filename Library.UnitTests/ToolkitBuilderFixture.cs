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

    public class ToolkitBuilderFixture
    {
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
    }
}