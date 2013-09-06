namespace NuPattern.Proxy
{
    using System;
    using System.Linq;

    internal static class Behaviors
    {
        public static readonly IBehavior[] Default = new IBehavior[]
        {
            new ProceedForTransparentProxy(),
            new GetEnumeratorForCollection(),
            new ThrowForNonPropertyAccess(),
            new GetComponentName(),
            new SetComponentName(),
            new GetPropertyValue(),
            new SetPropertyValue(),
            new GetProxyForComponentReference(),
            new ThrowNotSupportedFallback(),
        };
    }
}