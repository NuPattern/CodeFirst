namespace NuPattern.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NuPattern.Properties;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal class JsonProductSerializer : IProductSerializer
    {
        private static readonly string CurrentVersionString = typeof(Resources)
            .Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        private static readonly Version CurrentVersion = new Version(CurrentVersionString);

        public void Serialize(TextWriter writer, IEnumerable<Product> products)
        {
            Guard.NotNull(() => writer, writer);
            Guard.NotNull(() => products, products);

            var json = new JsonTextWriter(writer);
            json.Formatting = Formatting.Indented;
            var visitor = new JsonWriterVisitor(json);

            json.WriteStartObject();
            json.WritePropertyName("$format");
            json.WriteValue(CurrentVersionString);

            foreach (var product in products)
            {
                product.Accept(visitor);
            }

            json.WriteEndObject();
            json.Flush();
        }

        public IEnumerable<Product> Deserialize(TextReader reader)
        {
            var store = (JObject)JsonSerializer.CreateDefault().Deserialize(new JsonTextReader(reader));
            var format = store.Property("$format");
            if (format == null)
                throw new ArgumentException(Strings.JsonProductSerializer.MissingFormat);

            Version version;
            if (format.Value.Type != JTokenType.String || !Version.TryParse((string)format.Value, out version))
                throw new ArgumentException(Strings.JsonProductSerializer.MissingFormat);

            if (version < CurrentVersion)
            {
                // TODO: apply format version migrations.
            }

            foreach (var productProp in store.Properties().Where(x => 
                !x.Name.StartsWith("$") && x.HasValues && x.Value.Type == JTokenType.Object))
            {
                var productJson = (JObject)productProp.Value;
                var productSchemaId = GetSchema(productJson);
                var product = new Product(productProp.Name, productSchemaId);
                var toolkitProp = productJson.Property("$toolkit");
                if (toolkitProp == null || !toolkitProp.HasValues || toolkitProp.Value.Type != JTokenType.Object)
                    throw new ArgumentException(Strings.JsonProductSerializer.MissingToolkit);

                var toolkitJson = (JObject)toolkitProp.Value;
                var toolkitIdProp = toolkitJson.Property("$id");
                var toolkitVersionProp = toolkitJson.Property("$version");

                if (toolkitIdProp == null || !toolkitIdProp.HasValues || toolkitIdProp.Value.Type != JTokenType.String)
                    throw new ArgumentException(Strings.JsonProductSerializer.MissingToolkitId);

                if (toolkitVersionProp == null || !toolkitVersionProp.HasValues || toolkitVersionProp.Value.Type != JTokenType.String)
                    throw new ArgumentException(Strings.JsonProductSerializer.MissingToolkitVersion);

                product.Toolkit.Id = (string)toolkitIdProp.Value;
                product.Toolkit.Version = SemanticVersion.Parse((string)toolkitVersionProp.Value);

                DeserializeContainer(product, productJson);

                yield return product;
            }
        }

        private void DeserializeElement(IElement element, JObject json)
        {
            DeserializeContainer(element, json);
        }

        private void DeserializeCollection(ICollection collection, JObject json)
        {
            DeserializeContainer(collection, json);

            var itemsProp = json.Property("$items");
            if (!itemsProp.HasValues || itemsProp.Value.Type != JTokenType.Object)
                throw new ArgumentException(Strings.JsonProductSerializer.InvalidItems);

            var itemsJson = (JObject)itemsProp.Value;
            foreach (var itemProp in itemsJson.Properties().Where(x =>
                !x.Name.StartsWith("$") && x.HasValues && x.Value.Type == JTokenType.Object))
            {
                var itemJson = (JObject)itemProp.Value;
                var itemSchemaId = GetSchema(itemJson);
                DeserializeElement(collection.CreateItem(itemProp.Name, itemSchemaId), itemJson);
            }
        }

        private void DeserializeContainer(IContainer container, JObject json)
        {
            DeserializeComponent(container, json);

            foreach (var componentProp in json.Properties().Where(x =>
                !x.Name.StartsWith("$") && x.HasValues && x.Value.Type == JTokenType.Object))
            {
                var componentJson = (JObject)componentProp.Value;
                var componentSchemaId = GetSchema(componentJson);

                if (componentJson.Property("$items") != null)
                {
                    DeserializeCollection(container.CreateCollection(componentProp.Name, componentSchemaId), componentJson);
                }
                else
                {
                    DeserializeElement(container.CreateElement(componentProp.Name, componentSchemaId), componentJson);
                }
            }
        }

        private void DeserializeComponent(IComponent component, JObject json)
        {
            foreach (var property in json.Properties().Where(x =>
                !x.Name.StartsWith("$") && x.HasValues && x.Value.Type != JTokenType.Object))
            {
                var jvalue = property.Value as JValue;
                if (jvalue != null)
                {
                    var value = jvalue.Value;
                    var str = value as string;
                    Guid guid;
                    // See if it's a Guid.
                    if (str != null && str.StartsWith("{") && str.EndsWith("}") &&
                        Guid.TryParse(str, out guid))
                    {
                        value = guid;
                    }

                    component.CreateProperty(property.Name).Value = value;
                }
            }
        }

        private string GetSchema(JObject component)
        {
            var schema = component.Property("$schema");
            if (schema == null || !schema.HasValues || schema.Value.Type != JTokenType.String)
                throw new ArgumentException(Strings.JsonProductSerializer.MissingSchema);

            return (string)schema.Value;
        }

        private class JsonWriterVisitor : InstanceVisitor
        {
            private JsonWriter writer;

            public JsonWriterVisitor(JsonWriter writer)
            {
                this.writer = writer;
            }

            public override InstanceVisitor VisitProduct(IProduct product)
            {
                writer.WritePropertyName(product.Name);
                writer.WriteStartObject();
                WriteSchemaId(product);

                base.VisitProduct(product);

                writer.WritePropertyName("$toolkit");
                writer.WriteStartObject();
                {
                    writer.WritePropertyName("$id");
                    writer.WriteValue(product.Toolkit.Id);
                    writer.WritePropertyName("$version");
                    writer.WriteValue(product.Toolkit.Version.ToString());
                }
                writer.WriteEndObject();

                writer.WriteEndObject();
                writer.Flush();

                return this;
            }

            public override InstanceVisitor VisitElement(IElement element)
            {
                writer.WritePropertyName(element.Name);
                writer.WriteStartObject();
                WriteSchemaId(element);

                base.VisitElement(element);

                writer.WriteEndObject();

                return this;
            }

            public override InstanceVisitor VisitCollection(ICollection collection)
            {
                writer.WritePropertyName(collection.Name);
                writer.WriteStartObject();
                WriteSchemaId(collection);

                base.VisitCollection(collection);

                writer.WriteEndObject();

                return this;
            }

            protected override void VisitCollectionItems(IEnumerable<IElement> items)
            {
                writer.WritePropertyName("$items");
                writer.WriteStartObject();

                base.VisitCollectionItems(items);

                writer.WriteEndObject();
            }

            protected override void VisitProperty(IProperty property)
            {
                if (property.ShouldSerializeValue)
                {
                    writer.WritePropertyName(property.Name);
                    if (property.Value is Guid)
                    {
                        writer.WriteValue(((Guid)property.Value).ToString("B"));
                    }
                    else
                    {
                        writer.WriteValue(property.Value);
                    }
                }

                base.VisitProperty(property);
            }

            private void WriteSchemaId(IComponent component)
            {
                writer.WritePropertyName("$schema");
                writer.WriteValue(component.SchemaId);
            }
        }
    }
}