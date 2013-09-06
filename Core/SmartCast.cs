namespace NuPattern
{
    using Castle.DynamicProxy;
    using NuPattern.Properties;
    using NuPattern.Proxy;
    using System;
    using System.Linq;

    internal class SmartCast
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public object Cast(IComponent component, Type targetType)
        {
            if (!targetType.IsInterface)
                throw new ArgumentException();

            var propertyName = "_" + targetType.AssemblyQualifiedName;
            var proxyProperty = component.Properties.FirstOrDefault(p => p.Name == propertyName);
            if (proxyProperty == null)
            {
                if (!IsCompatible(component, targetType))
                    return null;

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

        private bool IsCompatible(IComponent component, Type type)
        {
            return type.GetProperties().Where(info => info.Name != "Name" && info.PropertyType.IsNative()).All(info =>
                component.Properties.Any(prop =>
                    prop.Name == info.Name &&
                    prop.Schema != null &&
                    prop.Schema.PropertyType == info.PropertyType));
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