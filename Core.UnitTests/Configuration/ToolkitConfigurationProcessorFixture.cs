namespace NuPattern.Configuration
{
    using Autofac;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using NetFx.StringlyTyped;

    public class ToolkitConfigurationProcessorFixture
    {
        [Fact]
        public void when_dispatching_configuration_then_resolves_configurators()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ToolkitSchemaConfigurator>().AsImplementedInterfaces();
            builder.RegisterType<ProductSchemaConfigurator>().AsImplementedInterfaces();
            builder.RegisterType<CollectionSchemaConfigurator>().AsImplementedInterfaces();
            builder.RegisterType<ElementSchemaConfigurator>().AsImplementedInterfaces();
            builder.RegisterType<PropertySchemaConfigurator>().AsImplementedInterfaces();
            builder.RegisterType<EventAutomationConfigurator>().AsImplementedInterfaces();

            var container = builder.Build();

            var schema = new ToolkitSchema("Toolkit", "1.0")
            {
                Products =
                {
                    new ProductSchema(typeof(IProduct).ToTypeFullName())
                    {
                        Components = 
                        {
                            new ElementSchema(typeof(IElement).ToTypeFullName())
                            {
                            },
                            new CollectionSchema(typeof(ICollection).ToTypeFullName())
                            {
                                ItemSchema = new ElementSchema(typeof(IItem).ToTypeFullName())
                                {
                                }
                            }
                        },
                        Properties = 
                        {
                            new PropertySchema("Port", typeof(int))
                        },
                    }
                }
            };

            var config = new ToolkitConfiguration("Toolkit", "1.0");
            var product = config.Product(typeof(IProduct));
            product.Properties.Add(new PropertyConfiguration());
            product.Automations.Add(new EventConfiguration { EventType = typeof(IObservable<EventArgs>) });

            var dispatcher = new ToolkitConfigurationProcessor(new ComponentContext(container));

            dispatcher.Process(schema, config);
        }

        public interface IProduct
        {
            int Port { get; set; }
            IElement Element { get; set; }
            ICollection Collection { get; set; }
        }

        public interface IElement { }
        public interface IItem { }
        public interface ICollection : IEnumerable<IItem> { }
    }
}