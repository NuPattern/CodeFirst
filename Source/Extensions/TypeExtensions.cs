namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TypeExtensions
    {
        /// <summary>
        /// Our notion of native values is wider than .NET Type.IsPrimitive, 
        /// since we include Guids and strings.
        /// </summary>
        public static bool IsNative(this Type type)
        {
            return type == typeof(Guid) || Type.GetTypeCode(type) != TypeCode.Object;
        }

        /// <summary>
        /// We consider a collection a straight IEnumerable{T} or an interface 
        /// that inherits from it.
        /// </summary>
        public static bool IsCollection(this Type type)
        {
            return IsEnumerable(type) || type.GetInterfaces().Any(i => IsEnumerable(i));
        }

        /// <summary>
        /// Retrieves the collection item type.
        /// </summary>
        public static Type GetItemType(this Type type)
        {
            if (IsEnumerable(type))
                return type.GetGenericArguments()[0];

            return type.GetInterfaces().First(i => IsEnumerable(i))
                .GetGenericArguments()[0];
        }

        private static bool IsEnumerable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }
    }
}