namespace NuPattern.Proxy
{
    using Castle.DynamicProxy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class GetEnumeratorForCollection : IBehavior
    {
        // Cache compiled enumerator factories to avoid reflection invoke on 
        // every collection enumeration. Note that we cannot cache the 
        // enumerator itself on the collection, since the enumerator is 
        // by definition transient while enumeration is performed.
        private static readonly Dictionary<Type, Func<ICollection, object>> enumeratorFactories =
            new Dictionary<Type, Func<ICollection, object>>();

        private static readonly MethodInfo genericCast = typeof(GetEnumeratorForCollection)
            .GetMethod("Cast", BindingFlags.NonPublic | BindingFlags.Static);

        public bool AppliesTo(IInvocation invocation)
        {
            return invocation.Method.Name == "GetEnumerator" &&
                invocation.Method.DeclaringType.IsGenericType &&
                invocation.Method.DeclaringType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                invocation.Proxy is ICollection;
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            var collection = (ICollection)invocation.Proxy;
            var itemType = invocation.Method.DeclaringType.GetGenericArguments()[0];

            invocation.ReturnValue = enumeratorFactories
                .GetOrAdd(itemType, t => CreateFactory(t))
                .Invoke(collection);

            return BehaviorAction.Stop;
        }

        private Func<ICollection, object> CreateFactory(Type itemType)
        {
            var param = Expression.Parameter(typeof(ICollection), "collection");
            var lambda = Expression.Lambda<Func<ICollection, object>>(
                Expression.Call(null, genericCast.MakeGenericMethod(itemType), param),
                param);

            return lambda.Compile();
        }

        private static IEnumerator<T> Cast<T>(ICollection collection)
        {
            var sc = new SmartCast();

            return collection.Items.Select(e => (T)sc.Cast(e, typeof(T))).GetEnumerator();
        }
    }
}