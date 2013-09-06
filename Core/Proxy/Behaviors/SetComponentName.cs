namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;
    using System.Linq;

    internal class SetComponentName : IBehavior
    {
        public bool AppliesTo(IInvocation invocation)
        {
            return invocation.Method.Name == "set_Name";
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            ((IComponent)invocation.Proxy).Name = (string)invocation.Arguments[0];

            return BehaviorAction.Stop;
        }
    }
}