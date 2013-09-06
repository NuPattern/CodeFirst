namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;

    internal interface IBehavior
    {
        bool AppliesTo(IInvocation invocation);
        BehaviorAction ExecuteFor(IInvocation invocation);
    }
}