namespace NuPattern
{
    using System;
    using System.Linq;

    internal class ValueHandler
    {
        public static object Get(Property property)
        {
            var value = property.GetValue();
            if (value == null && property.Schema != null && 
                property.Schema.PropertyType.IsValueType && 
                !IsNullableType(property.Schema.PropertyType))
            {
                value = Activator.CreateInstance(property.Schema.PropertyType);
            }

            return value;
        }

        public static void Set(Property property, object value)
        {
            property.SetValue(value);
        }

        public static bool ShouldSerialize(Property property)
        {
            // TODO: should only return true if the property 
            // value is different than the default value for 
            // it, either because of schema-specified default 
            // value or from property type default value.
            return property.Value != null;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}