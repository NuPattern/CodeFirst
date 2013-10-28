namespace NuPattern
{
    using System;
    using System.Linq;

    public class ValueHandler
    {
        public static object Get(Property property)
        {
            // TODO: consider DefaultValueAttribute?
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
            var oldValue = Get(property);

            // TODO: perform type conversion, etc.?
            // TODO: do nothing if property name starts with "$" or "_"?
            if (!Object.Equals(oldValue, value))
            {
                property.Owner.Events.RaisePropertyChanging(property.Name, oldValue, value);
                property.SetValue(value);
                property.Owner.Events.RaisePropertyChanged(property.Name, oldValue, value);
            }
        }

        public static bool ShouldSerialize(Property property)
        {
            if (property.Name.StartsWith("_"))
                return false;

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