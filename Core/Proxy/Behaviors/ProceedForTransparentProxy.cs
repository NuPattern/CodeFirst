namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;
    using System.Linq;

    /// <summary>
    /// This behavior allows the transparent proxying of the underlying 
    /// runtime interfaces on top of the custom product interaces. 
    /// We let all of these calls to go through to the underlying 
    /// target, which is the real runtime object.
    /// </summary>
    public class ProceedForTransparentProxy : IBehavior
    {
        public bool AppliesTo(IInvocation invocation)
        {
            return invocation.Method.DeclaringType.Assembly == typeof(IComponent).Assembly;
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            invocation.Proceed();

            return BehaviorAction.Stop;
        }
    }
}