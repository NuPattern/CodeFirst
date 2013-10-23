namespace NuPattern
{
    using NuPattern.Core.Properties;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public abstract class PropertyBinding
    {
        private static ConcurrentDictionary<Tuple<Type, string>, Action<object, object>> setters = new ConcurrentDictionary<Tuple<Type, string>, Action<object, object>>();

        private Type instanceType;
        private PropertyInfo propertyInfo;
        private Lazy<Action<object, object>> setter;

        protected PropertyBinding(Type instanceType, string propertyName)
        {
            Guard.NotNull(() => instanceType, instanceType);
            Guard.NotNullOrEmpty(() => propertyName, propertyName);

            this.propertyInfo = instanceType.GetProperty(propertyName);
            if (this.propertyInfo == null)
                throw new ArgumentException(Strings.Binding.PropertyNotFound(propertyName, instanceType));

            this.instanceType = instanceType;
            this.PropertyName = propertyName;
            this.setter = new Lazy<Action<object, object>>(() => setters.GetOrAdd(Tuple.Create(instanceType, propertyName), BuildSetter));
        }

        public string PropertyName { get; private set; }

        public abstract void Refresh(object instance);

        protected void SetValue(object instance, object value)
        {
            setter.Value.Invoke(instance, value);
        }

        private Action<object, object> BuildSetter(Tuple<Type, string> key)
        {
            var instanceParam = Expression.Parameter(typeof(object), "instance");
            var valueParam = Expression.Parameter(typeof(object), "value");
            var lambda = Expression.Lambda<Action<object, object>>(
                Expression.Assign(
                    Expression.MakeMemberAccess(
                        Expression.Convert(instanceParam, key.Item1),
                        propertyInfo),
                    Expression.Convert(valueParam, propertyInfo.PropertyType)),
                instanceParam, valueParam);

            return lambda.Compile();
        }
    }
}