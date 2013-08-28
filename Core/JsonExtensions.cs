namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    internal static class JsonExtensions
    {
        private static MethodInfo getValue = typeof(JObject).GetMethod("GetValue", new[] { typeof(string) });
        private static IDictionary<Type, object> getters;

        static JsonExtensions()
        {
            var operators = typeof(JToken).GetMethods().Where(m => m.Name == "op_Explicit");
            var funcType = typeof(Func<,,>);
            getters = operators
                .Select(op => new
                {
                    LambdaType = funcType.MakeGenericType(
                        typeof(JObject), typeof(string), op.ReturnType),
                    ReturnType = op.ReturnType,
                })
                .ToDictionary(
                    op => op.ReturnType,
                    op => CompileGetter(op.LambdaType, op.ReturnType));
        }

        public static bool IsNative(Type valueType)
        {
            return getters.ContainsKey(valueType);
        }

        public static Product AsProduct(this JObject component)
        {
            return (Product)component.GetModel() ?? new Product(component);
        }

        public static Product AsExtension(this JObject component, JProperty property)
        {
            return (Product)component.GetModel() ?? new Product(component, property);
        }

        public static Component AsComponent(this JObject component)
        {
            return AsComponent(component, null);
        }

        public static Component AsComponent(this JObject component, JProperty property)
        {
            var result = (Component)component.GetModel();
            if (result == null)
            {
                var isProductOrExtension = component.Property(Prop.Toolkit) != null;
                var isCollection = component.Property(Prop.Items) != null;
                if (isProductOrExtension)
                    return new Product(component, property);
                else if (isCollection)
                    return new Collection(component, property);
                else
                    return new Element(component, property);
            }

            return result;
        }

        public static Collection AsCollection(this JObject component)
        {
            return (Collection)component.GetModel() ?? new Collection(component);
        }

        public static Element AsElement(this JObject component)
        {
            return (Element)component.GetModel() ?? new Element(component);
        }

        public static object GetModel(this JObject json)
        {
            var property = json.Property("$model");
            if (property == null)
                return null;

            return ((JValue)property.Value).Value;
        }

        public static void SetModel(this JObject json, object model)
        {
            var property = json.Property("$model");
            if (property == null)
                json.Add(new JTransientProperty("$model", model));
            else
                property.Value = new JRaw(model);
        }

        public static T Get<T>(this JObject json, Expression<Func<T>> property)
        {
            if (property.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("Invalid property expression " + property);

            var propertyName = ((MemberExpression)property.Body).Member.Name;

            return Get<T>(json, propertyName);
        }

        public static void Set<T>(this JObject json, Expression<Func<T>> property, T value)
        {
            if (property.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("Invalid property expression " + property);

            var propertyName = ((MemberExpression)property.Body).Member.Name;
            Set<T>(json, propertyName, value);
        }

        public static T Get<T>(this JObject json, string propertyName)
        {
            object getter = null;
            if (!getters.TryGetValue(typeof(T), out getter))
            {
                // Special case for enums
                if (typeof(T).IsEnum)
                {
                    var enumGetter = (Func<JObject, string, int>)getters[typeof(int)];
                    var enumValue = enumGetter.Invoke(json, propertyName);
                    return (T)Enum.ToObject(typeof(T), enumValue);
                }

                throw new ArgumentException(string.Format(
                        CultureInfo.InvariantCulture,
                        "Could not determine JSON object type for type {0} from property {1}.", typeof(T), propertyName));
            }

            return ((Func<JObject, string, T>)getter).Invoke(json, propertyName);
        }

        public static void Set<T>(this JObject json, string propertyName, T value)
        {
            var jprop = json.Property(propertyName);

            if (value == null)
            {
                if (jprop != null)
                    jprop.Remove();

                return;
            }

            if (jprop == null)
            {
                jprop = new JProperty(propertyName, value);
                json.Add(jprop);
            }
            else
            {
                jprop.Value = new JValue(value);
            }
        }

        private static object CompileGetter(Type funcType, Type returnType)
        {
            var jParam = Expression.Parameter(typeof(JObject), "json");
            var nameParam = Expression.Parameter(typeof(string), "propertyName");
            var expression = Expression.Lambda(
                funcType,
                Expression.Condition(
                    Expression.Equal(Expression.Call(jParam, getValue, nameParam), Expression.Constant(null)),
                    Expression.Default(returnType),
                    Expression.Convert(
                        Expression.Call(jParam, getValue, nameParam),
                        returnType)),
                jParam, nameParam);

            return expression.Compile();
        }
    }
}