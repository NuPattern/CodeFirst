namespace NuPattern.Proxy
{
    using NuPattern;
    using Castle.DynamicProxy;
    using System;
    using System.Linq;

    internal class GetPropertyValue : IBehavior
    {
        public bool AppliesTo(IInvocation invocation)
        {
            return invocation.Method.IsSpecialName &&
                invocation.Method.Name.StartsWith("get_") && 
                invocation.Method.ReturnType.IsNative();
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            var component = (IComponent)invocation.Proxy;
            var propertyName = invocation.Method.Name.Substring(4);
            var property = component.Properties.FirstOrDefault(p => p.Name == propertyName);
            if (property != null)
            {
                invocation.ReturnValue = property.Value;
                return BehaviorAction.Stop;
            }

            return BehaviorAction.Continue;
        }
    }
}