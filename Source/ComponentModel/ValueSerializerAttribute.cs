namespace NuPattern.ComponentModel
{
    using System;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Class)]
    public class ValueSerializerAttribute : Attribute
    {
        public ValueSerializerAttribute(string typeName)
        {
            this.SerializerTypeName = typeName;
        }

        public ValueSerializerAttribute(Type type)
        {
            this.SerializerTypeName = type.AssemblyQualifiedName;
        }

        public string SerializerTypeName { get; private set; }
    }
}