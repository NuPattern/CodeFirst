namespace NuPattern.Schema
{
    using NetFx.StringlyTyped;
    using NuPattern.Properties;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SchemaBuilder
    {
        public IProductSchema BuildProduct(IToolkitSchema toolkit, Type productType)
        {
            if (!productType.IsInterface)
                throw new ArgumentException(Strings.SchemaBuilder.ModelMustBeInterfaces(productType));

            var schema = toolkit.CreateProductSchema(productType.ToTypeFullName());

            BuildType(productType, schema);

            return schema;
        }

        private void BuildType(Type type, IContainerSchema schema)
        {
            foreach (var property in type.GetProperties().Where(x => Type.GetTypeCode(x.PropertyType) != TypeCode.Object))
            {
                schema.CreatePropertySchema(property.Name, property.PropertyType);
            }

            foreach (var property in type.GetProperties().Where(x => Type.GetTypeCode(x.PropertyType) == TypeCode.Object))
            {
                if (!property.PropertyType.IsInterface)
                    throw new ArgumentException(Strings.SchemaBuilder.ModelMustBeInterfaces(property.PropertyType));

                if (IsCollection(property.PropertyType))
                {
                    var collectionSchema = schema.CreateCollectionSchema(property.PropertyType.ToTypeFullName());
                    var itemType = GetElementType(property.PropertyType);
                    var itemSchema = collectionSchema.CreateItemSchema(itemType.ToTypeFullName());

                    BuildType(itemType, itemSchema);
                    BuildType(property.PropertyType, collectionSchema);
                }
                else
                {
                    BuildType(property.PropertyType, schema.CreateElementSchema(property.PropertyType.ToTypeFullName()));
                }
            }
        }

        private bool IsCollection(Type type)
        {
            return IsEnumerable(type) || type.GetInterfaces().Any(i => IsEnumerable(i));
        }

        private static bool IsEnumerable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        private Type GetElementType(Type type)
        {
            if (IsEnumerable(type))
                return type.GetGenericArguments()[0];

            return type.GetInterfaces().First(i => IsEnumerable(i))
                .GetGenericArguments()[0];
        }
    }
}
