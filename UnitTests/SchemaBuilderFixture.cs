namespace NuPattern.SchemaBuilderFixture
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using Xunit;
    using NetFx.StringlyTyped;
    using System.Collections;
    using System.Collections.Generic;

    public class given_a_model
    {
        [Fact]
        public void when_building_product_then_exposes_product_properties()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.NotNull(schema);
            Assert.Equal(3, schema.PropertySchemas.Count());

            var accessKeyProp = schema.PropertySchemas.First(x => x.Name == "AccessKey");
            var isPublicProp = schema.PropertySchemas.First(x => x.Name == "IsPublic");
            var portProp = schema.PropertySchemas.First(x => x.Name == "Port");


            Assert.True(schema.PropertySchemas.Any(x => x.Name == "AccessKey" && x.Type == typeof(string)));
            Assert.True(schema.PropertySchemas.Any(x => x.Name == "IsPublic" && x.Type == typeof(bool)));
            Assert.True(schema.PropertySchemas.Any(x => x.Name == "Port" && x.Type == typeof(int)));
        }

        [Fact]
        public void when_product_type_has_reference_property_then_creates_element_schema()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.False(schema.PropertySchemas.Any(x => x.Name == "MyElement"));

            var element = schema.ComponentSchemas.FirstOrDefault();

            Assert.NotNull(element);
            Assert.Equal(typeof(IMyElement).ToTypeFullName(), element.SchemaId);
            Assert.True(element.PropertySchemas.Any(x => x.Name == "Location" && x.Type == typeof(string)));
        }

        [Fact]
        public void when_product_type_has_enumerable_property_then_creates_collection_schema()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.False(schema.PropertySchemas.Any(x => x.Name == "MyElement"));

            var collection = schema.ComponentSchemas.OfType<ICollectionSchema>().FirstOrDefault();

            Assert.NotNull(collection);
            Assert.NotNull(collection.ItemSchema);
            Assert.Equal(typeof(IMyItem).ToTypeFullName(), collection.ItemSchema.SchemaId);
            Assert.Equal(typeof(IEnumerable<IMyItem>).ToTypeFullName(), collection.SchemaId);
            Assert.True(collection.ItemSchema.PropertySchemas.Any(x => x.Name == "Path" && x.Type == typeof(string)));
        }

        [Fact]
        public void when_product_type_has_custom_enumerable_property_then_creates_collection_schema()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.False(schema.PropertySchemas.Any(x => x.Name == "MyCustomItems"));

            var collection = schema.ComponentSchemas.OfType<ICollectionSchema>()
                .FirstOrDefault(x => x.SchemaId.EndsWith("IMyItems"));

            Assert.NotNull(collection);
            Assert.NotNull(collection.ItemSchema);
            Assert.Equal(typeof(IMyItem).ToTypeFullName(), collection.ItemSchema.SchemaId);
            Assert.Equal(typeof(IMyItems).ToTypeFullName(), collection.SchemaId);
            Assert.True(collection.PropertySchemas
                .Any(x => x.Name == "IsSafe" && x.Type == typeof(bool)));
            Assert.True(collection.ItemSchema.PropertySchemas
                .Any(x => x.Name == "Path" && x.Type == typeof(string)));
        }
    }

    public interface IMyProduct
    {
        string AccessKey { get; set; }
        bool IsPublic { get; set; }
        int Port { get; set; }
        IMyElement MyElement { get; set; }
        IEnumerable<IMyItem> MyItems { get; }
        IMyItems MyCustomItems { get; }
    }

    public interface IMyElement
    {
        string Location { get; set; }
    }

    public interface IMyItem
    {
        string Path { get; set; }
    }

    public interface IMyItems : IEnumerable<IMyItem>
    {
        bool IsSafe { get; set; }
    }
}