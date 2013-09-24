namespace NuPattern
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public static class IComponentContextExtensions
    {
        public static T Instantiate<T>(this IComponentContext context)
        {
            return (T)context.Instantiate(typeof(T));
        }

        public static T Resolve<T>(this IComponentContext context)
        {
            return (T)context.Resolve(typeof(T));
        }

        public static T ResolveOptional<T>(this IComponentContext context)
        {
            return (T)context.ResolveOptional(typeof(T));
        }
    }
}