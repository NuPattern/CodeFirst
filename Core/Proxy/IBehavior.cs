namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;

    public interface IBehavior
    {
        bool AppliesTo(IInvocation invocation);
        BehaviorAction ExecuteFor(IInvocation invocation);
    }
}