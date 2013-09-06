namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;
    using System.Linq;

    internal class ThrowNotSupportedFallback : IBehavior
    {
        public bool AppliesTo(IInvocation invocation)
        {
            return true;
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            throw new NotSupportedException();
        }
    }
}