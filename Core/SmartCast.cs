namespace NuPattern
{
    using Castle.DynamicProxy;
    using NuPattern.Properties;
    using NuPattern.Proxy;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    internal class SmartCast
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public object Cast(IComponent component, Type targetType)
        {
            Guard.NotNull(() => component, component);
            Guard.NotNull(() => targetType, targetType);

            if (!targetType.IsInterface)
                throw new ArgumentException(Strings.SmartCast.InterfaceRequired);

            if (component.Schema == null)
                throw new ArgumentException(Strings.SmartCast.SchemaRequired(component));

            var propertyName = "$" + targetType.AssemblyQualifiedName;
            // NOTE: we cache the proxy instance for performance.
            var proxyProperty = component.Properties.FirstOrDefault(p => p.Name == propertyName);
            if (proxyProperty == null)
            {
                var containerSchema = component.Schema as IContainerInfo;
                if (containerSchema != null)
                {
                    if (!IsCompatibleContainer(containerSchema, targetType))
                        return null;
                }
                else if (!IsCompatibleComponent(component.Schema, targetType))
                {
                    return null;
                }

                proxyProperty = component.CreateProperty(propertyName);
                // Try to expose the most specific interface of the received component.
                var componentType = component is IProduct ? typeof(IProduct) :
                    (component is IElement ? typeof(IElement) :
                    (component is ICollection ? typeof(ICollection) : typeof(IComponent)));

                proxyProperty.Value = generator.CreateInterfaceProxyWithTarget(
                    componentType, new[] { targetType }, component, new Interceptor(component, Behaviors.Default));
            }

            return proxyProperty.Value;
        }

        private bool IsCompatibleComponent(IComponentInfo schema, Type type)
        {
            // Property comparison.
            return type.GetProperties().Where(info => info.Name != "Name" && info.PropertyType.IsNative()).All(info =>
                schema.Properties.Any(prop =>
                    prop.Name == info.Name &&
                    prop.PropertyType == info.PropertyType));
        }

        private bool IsCompatibleContainer(IContainerInfo schema, Type type)
        {
            // Initial property comparison.
            if (!IsCompatibleComponent(schema, type))
                return false;

            var referenceProperties = type.GetProperties().Where(info => !info.PropertyType.IsNative());

            // Check compatibility of elements.
            if (!referenceProperties.Where(info => !info.PropertyType.IsCollection()).All(info =>
                schema.Components.OfType<IElementInfo>().Any(element =>
                    element.DefaultName == info.Name &&
                    IsCompatibleContainer(element, info.PropertyType))))
                return false;

            // Check compatibility of collections
            return referenceProperties.Where(info => info.PropertyType.IsCollection()).All(info =>
                schema.Components.OfType<ICollectionInfo>().Any(collection =>
                    collection.DefaultName == info.Name &&
                    IsCompatibleCollection(collection, info.PropertyType)));
        }

        private bool IsCompatibleCollection(ICollectionInfo schema, Type type)
        {
            return IsCompatibleContainer(schema, type) &&
                IsCompatibleContainer(schema.Item, type.GetItemType());
        }

        private class Interceptor : IInterceptor
        {
            private IComponent component;
            private IBehavior[] behaviors;

            public Interceptor(IComponent component, IBehavior[] behaviors)
            {
                this.component = component;
                this.behaviors = behaviors;
            }

            public void Intercept(IInvocation invocation)
            {
                behaviors.Where(b => b.AppliesTo(invocation))
                    .TakeWhile(b => b.ExecuteFor(invocation) == BehaviorAction.Continue)
                    .ToArray();
            }
        }
    }
}