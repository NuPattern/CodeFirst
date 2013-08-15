﻿namespace NuPattern
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

        public static Product AsProduct(this JObject component)
        {
            return (Product)component.GetModel() ?? new Product(component);
        }

        public static Component AsComponent(this JObject component)
        {
            var result = (Component)component.GetModel();
            if (result == null)
            {
                var definition = (string)component.Property("Definition").Value;
                if (definition == "Element")
                    result = new Element(component);
                else if (definition == "Collection")
                    result = new Collection(component);
            }

            return result;
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

            object getter = null;
            if (!getters.TryGetValue(typeof(T), out getter))
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Could not determine JSON object type for type {0} from property {1}.", typeof(T), propertyName));

            return ((Func<JObject, string, T>)getter).Invoke(json, propertyName);
        }

        public static void Set<T>(this JObject json, Expression<Func<T>> property, T value)
        {
            if (property.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("Invalid property expression " + property);

            var propertyName = ((MemberExpression)property.Body).Member.Name;
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