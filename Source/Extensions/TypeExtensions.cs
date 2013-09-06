namespace NuPattern
{
    using System;
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
    }
}