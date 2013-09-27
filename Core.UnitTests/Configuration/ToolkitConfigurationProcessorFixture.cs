namespace NuPattern.Configuration
{
    using Autofac;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using NetFx.StringlyTyped;
    using NuPattern.Automation;
    using System.ComponentModel.DataAnnotations;
    using System.Reactive;

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
            builder.RegisterType<EventAutomationSettingsFactory>().AsImplementedInterfaces();
            builder.RegisterType<CommandAutomationSettingsFactory>().AsImplementedInterfaces();

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
            product.Automations.Add(new EventConfiguration 
            {
                EventType = typeof(IObservable<IEventPattern<object, EventArgs>>), 
                CommandConfiguration = new AnonymousCommandConfiguration<IProduct>(p => Console.WriteLine(p))
            });

            config.Accept(new ValidatorVisitor());

            var dispatcher = new ToolkitConfigurationService(new ComponentContext(container));

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

        private class ValidatorVisitor : IConfigurationVisitor
        {
            public void Visit<TConfiguration>(TConfiguration configuration)
            {
                Validator.ValidateObject(configuration, new ValidationContext(configuration, null, null), true);
            }
        }
    }
}