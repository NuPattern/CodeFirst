namespace NuPattern
{
    using System;
    using System.Linq;

    internal class ValueHandler
    {
        public static object Get(Property property)
        {
            return property.GetValue();
        }

        public static void Set(Property property, object value)
        {
            property.SetValue(value);
        }
    }
}