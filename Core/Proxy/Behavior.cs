namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;
    using System.Linq;

    public abstract class Behavior : IBehavior
    {
        public virtual bool AppliesTo(IInvocation invocation)
        {
            return true;
        }

        public abstract BehaviorAction ExecuteFor(IInvocation invocation);
    }
}