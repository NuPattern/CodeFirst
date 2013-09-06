namespace NuPattern
{
    using Castle.DynamicProxy;
    using NuPattern.Properties;
    using System;
    using System.Linq;

    internal class SmartCast
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public T As<T>(IComponent component)
            where T : class
        {
            if (!typeof(T).IsInterface)
                throw new ArgumentException();

            var propertyName = "_" + typeof(T).AssemblyQualifiedName;
            var proxyProperty = component.Properties.FirstOrDefault(p => p.Name == propertyName);
            if (proxyProperty == null)
            {
                if (!IsCompatible(component, typeof(T)))
                    return null;

                proxyProperty = component.CreateProperty(propertyName);
                proxyProperty.Value = generator.CreateInterfaceProxyWithoutTarget(typeof(T), new[] { typeof(IProxied) }, new Interceptor(component));
            }

            return (T)proxyProperty.Value;
        }

        public IComponent AsComponent(object instance)
        {
            var proxied = instance as IProxied;
            if (proxied == null)
                throw new ArgumentException(Strings.SmartCast.InstanceNotProductInterface);

            return proxied.Component;
        }

        private bool IsCompatible(IComponent component, Type type)
        {
            return type.GetProperties().Where(info => info.Name != "Name").All(info =>
                component.Properties.Any(prop =>
                    prop.Name == info.Name &&
                    prop.Schema != null &&
                    prop.Schema.PropertyType == info.PropertyType));
        }

        private class Interceptor : IInterceptor
        {
            private IComponent component;

            public Interceptor(IComponent component)
            {
                this.component = component;
            }

            public void Intercept(IInvocation invocation)
            {
                // Special name already shortcircuits everything that 
                // is not a get_ or set_.
                if (!invocation.Method.IsSpecialName)
                    throw new NotSupportedException();

                if (invocation.Method.DeclaringType == typeof(IProxied))
                {
                    invocation.ReturnValue = component;
                    return;
                }

                if (invocation.Method.Name.StartsWith("get_"))
                {
                    var propertyName = invocation.Method.Name.Substring(4);
                    if (propertyName == "Name")
                    {
                        invocation.ReturnValue = component.Name;
                    }
                    else
                    {
                        var property = component.Properties.FirstOrDefault(p => p.Name == propertyName);
                        if (property != null)
                            invocation.ReturnValue = property.Value;
                    }

                    return;
                }

                if (invocation.Method.Name.StartsWith("set_"))
                {
                    var propertyName = invocation.Method.Name.Substring(4);
                    if (propertyName == "Name")
                    {
                        component.Name = (string)invocation.GetArgumentValue(0);
                    }
                    else
                    {
                        var property = component.Properties.FirstOrDefault(p => p.Name == propertyName);
                        if (property != null)
                            property.Value = invocation.GetArgumentValue(0);
                    }

                    return;
                }


                throw new NotSupportedException();
            }
        }
    }

    internal interface IProxied
    {
        IComponent Component { get; }
    }
}