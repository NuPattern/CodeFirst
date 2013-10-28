namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;
    using System.Linq;

    public class GetProxyForComponentReference : IBehavior
    {
        public bool AppliesTo(IInvocation invocation)
        {
            return invocation.Method.IsSpecialName &&
                invocation.Method.Name.StartsWith("get_") &&
                !invocation.Method.ReturnType.IsNative();
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            var container = (IContainer)invocation.Proxy;
            var propertyName = invocation.Method.Name.Substring(4);
            // Property name == component name.
            var component = container.Components.FirstOrDefault(c => c.Name == propertyName);

            if (component != null)
            {
                invocation.ReturnValue = new SmartCast().Cast(component, invocation.Method.ReturnType);
            }

            return BehaviorAction.Stop;
        }
    }
}