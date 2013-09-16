namespace NuPattern
{
    using NuPattern.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class JsonProductSerializerFixture
    {
        private Product[] products;

        public JsonProductSerializerFixture()
        {
            var product = new Product("MyProduct", "IProduct")
            {
                Toolkit = new ToolkitVersion
                {
                    Id = "MyToolkit",
                    Version = "1.0",
                }
            };

            product.CreateProperty("IsVisible").Value = true;
            // NOTE: default deserialization of this numeric value will be int64.
            // Schema should account for the truncation.
            product.CreateProperty("Number").Value = 23f;
            var element = product.CreateElement("MyElement", "IElement");
            element.CreateProperty("NullableNumber").Value = (int?)23;
            var collection = product.CreateCollection("MyCollection", "ICollection");
            var item = collection.CreateItem("MyItem", "IItem");
            item.CreateProperty("Guid").Value = Guid.NewGuid();

            products = new[] { product };
        }

        [Fact]
        public void when_serializing_product_then_succeeds()
        {
            var serializer = new JsonProductSerializer();

            serializer.Serialize(Console.Out, products);
        }

        [Fact]
        public void when_product_has_underscore_property_then_serialization_skips_it()
        {
            var serializer = new JsonProductSerializer();
            var writer = new StringWriter();

            products[0].CreateProperty("_culture").Value = CultureInfo.CurrentCulture;

            serializer.Serialize(writer, products);

            var deserialized = serializer.Deserialize(new StringReader(writer.ToString())).ToList();

            Assert.False(deserialized[0].Properties.Any(x => x.Name == "_culture"));
        }

        [Fact]
        public void when_serializing_product_then_can_roundtrip()
        {
            var serializer = new JsonProductSerializer();
            var writer = new StringWriter();

            serializer.Serialize(writer, products);

            var deserialized = serializer.Deserialize(new StringReader(writer.ToString())).ToList();

            Assert.NotNull(deserialized);

            var writer2 = new StringWriter();
            serializer.Serialize(writer2, deserialized);

            Assert.Equal(writer.ToString(), writer2.ToString());

            Assert.Equal(products[0], deserialized[0], ProductComparer.Default);
        }

        [Fact]
        public void when_deserializing_product_then_can_get_line_information()
        {
            var serializer = new JsonProductSerializer();
            var writer = new StringWriter();

            serializer.Serialize(writer, products);

            var deserialized = serializer.Deserialize(new StringReader(writer.ToString())).ToList();

            var lineInfo = deserialized[0] as ILineInfo;
            Assert.NotNull(lineInfo);
            Assert.True(lineInfo.HasLineInfo);
            Console.WriteLine("{0}, {1}", lineInfo.LineNumber, lineInfo.LinePosition);
        }

        private class ProductComparer : ContainerComparer<IProduct>
        {
            public static new EqualityComparer<IProduct> Default { get; private set; }

            static ProductComparer()
            {
                Default = new ProductComparer();
            }

            private ProductComparer()
            {
            }

            public override bool Equals(IProduct x, IProduct y)
            {
                return base.Equals(x, y) &&
                    x.Toolkit.Id == y.Toolkit.Id &&
                    x.Toolkit.Version == y.Toolkit.Version;
            }

            public override int GetHashCode(IProduct obj)
            {
                return obj.GetHashCode();
            }
        }

        private class ElementComparer : ContainerComparer<IElement>
        {
            public static new EqualityComparer<IElement> Default { get; private set; }

            static ElementComparer()
            {
                Default = new ElementComparer();
            }

            private ElementComparer()
            {
            }
        }

        private class CollectionComparer : ContainerComparer<ICollection>
        {
            public static new EqualityComparer<ICollection> Default { get; private set; }

            static CollectionComparer()
            {
                Default = new CollectionComparer();
            }

            private CollectionComparer()
            {
            }

            public override bool Equals(ICollection x, ICollection y)
            {
                return base.Equals(x, y) &&
                    x.Items.SequenceEqual(y.Items, ElementComparer.Default);
            }
        }

        private abstract class ContainerComparer<T> : EqualityComparer<T>
            where T : IContainer
        {
            public override bool Equals(T x, T y)
            {
                return x.SchemaId == y.SchemaId &&
                    x.Name == y.Name &&
                    x.Properties.SequenceEqual(y.Properties, PropertyComparer.Default) &&
                    x.Components.OfType<IElement>().SequenceEqual(y.Components.OfType<IElement>(), ElementComparer.Default) &&
                    x.Components.OfType<ICollection>().SequenceEqual(y.Components.OfType<ICollection>(), CollectionComparer.Default);
            }

            public override int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }

        private class PropertyComparer : EqualityComparer<IProperty>
        {
            public static new EqualityComparer<IProperty> Default { get; private set; }

            static PropertyComparer()
            {
                Default = new PropertyComparer();
            }

            private PropertyComparer()
            {
            }

            public override bool Equals(IProperty x, IProperty y)
            {
                return x.Name == y.Name &&
                    (x.Value.Equals(y.Value) ||
                    (AreNumeric(x, y) && AreNumericEquals(x, y)));
            }

            private static bool AreNumericEquals(IProperty x, IProperty y)
            {
                return
                    ((IConvertible)x.Value).ToDouble(CultureInfo.CurrentCulture).Equals(
                    ((IConvertible)y.Value).ToDouble(CultureInfo.CurrentCulture));
            }

            private static bool AreNumeric(IProperty x, IProperty y)
            {
                return IsNumeric(x.Value.GetType()) && IsNumeric(y.Value.GetType());
            }

            public override int GetHashCode(IProperty obj)
            {
                return obj.GetHashCode();
            }

            internal static bool IsNumeric(Type type)
            {
                if (!type.IsEnum)
                {
                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                    }
                }
                return false;
            }
        }
    }
}