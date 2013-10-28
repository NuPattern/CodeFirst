namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;
    using System.Linq;

    public class ThrowForNonPropertyAccess : IBehavior
    {
        public bool AppliesTo(IInvocation invocation)
        {
            // Special name already shortcircuits everything that 
            // is not a get_ or set_.
            return !invocation.Method.IsSpecialName;
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            throw new NotSupportedException();
        }
    }
}