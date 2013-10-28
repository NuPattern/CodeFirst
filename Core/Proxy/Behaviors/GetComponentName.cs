namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;
    using System.Linq;

    public class GetComponentName : IBehavior
    {
        public bool AppliesTo(IInvocation invocation)
        {
            return invocation.Method.Name == "get_Name";
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            invocation.ReturnValue = ((IComponent)invocation.Proxy).Name;

            return BehaviorAction.Stop;
        }
    }
}