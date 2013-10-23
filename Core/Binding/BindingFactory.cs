namespace NuPattern
{
    using CommonComposition;
    using NuPattern.Configuration;
    using NuPattern.Core.Properties;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Component(IsSingleton = true)]
    public class BindingFactory : IBindingFactory
    {
        public IBinding<T> CreateBinding<T>(IComponentContext context, BindingConfiguration configuration)
        {
            if (!typeof(T).IsAssignableFrom(configuration.Type))
                throw new ArgumentException(Strings.BindingFactory.IncompatibleBindingType(configuration.Type, typeof(T)));

            var instance = (T)context.Instantiate(configuration.Type);
            var properties = new List<PropertyBinding>();
            foreach (var property in configuration.Properties)
            {
                if (property.ValueProvider != null)
                {
                    properties.Add(new ProvidedPropertyBinding(configuration.Type, property.PropertyName,
                        CreateBinding<IValueProvider>(context, property.ValueProvider)));
                }
                else
                {
                    properties.Add(new ConstantPropertyBinding(configuration.Type, property.PropertyName, property.Value));
                }
            }

            return new Binding<T>(instance, properties);
        }
    }
}