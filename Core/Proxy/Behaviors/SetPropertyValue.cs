namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;
    using System.Linq;

    public class SetPropertyValue : IBehavior
    {
        public bool AppliesTo(IInvocation invocation)
        {
            return invocation.Method.IsSpecialName &&
                invocation.Method.Name.StartsWith("set_");
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            var component = (IComponent)invocation.Proxy;
            var propertyName = invocation.Method.Name.Substring(4);
            var property = component.Properties.FirstOrDefault(p => p.Name == propertyName);
            if (property != null)
            {
                property.Value = invocation.Arguments[0];
                return BehaviorAction.Stop;
            }

            return BehaviorAction.Continue;
        }
    }
}