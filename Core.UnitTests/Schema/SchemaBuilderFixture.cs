﻿namespace NuPattern.SchemaBuilderFixture
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using Xunit;
    using NetFx.StringlyTyped;
    using System.Collections;
    using System.Collections.Generic;
    using NuPattern.Configuration;

    public class given_a_model
    {
        [Fact]
        public void when_building_product_then_exposes_product_properties()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.NotNull(schema);
            Assert.Equal(3, schema.Properties.Count());

            Assert.True(schema.Properties.Any(x => x.Name == "AccessKey" && x.PropertyType == typeof(string)));
            Assert.True(schema.Properties.Any(x => x.Name == "IsPublic" && x.PropertyType == typeof(bool)));
            Assert.True(schema.Properties.Any(x => x.Name == "Port" && x.PropertyType == typeof(int)));
        }

        [Fact]
        public void when_product_type_has_reference_property_then_creates_element_schema()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.False(schema.Properties.Any(x => x.Name == "MyElement"));

            var element = schema.Components.FirstOrDefault();

            Assert.NotNull(element);
            Assert.Equal(typeof(IMyElement).ToTypeFullName(), element.SchemaId);
            Assert.True(element.Properties.Any(x => x.Name == "Location" && x.PropertyType == typeof(string)));
        }

        [Fact]
        public void when_product_type_has_reference_property_then_element_default_name_matches_property_name()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            var element = schema.Components.FirstOrDefault(c => c.SchemaId == typeof(IMyRenamedProperty).ToTypeFullName());

            Assert.NotNull(element);
            Assert.Equal("Other", element.DefaultName);
        }

        [Fact]
        public void when_product_type_has_enumerable_property_then_creates_collection_schema()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.False(schema.Properties.Any(x => x.Name == "MyElement"));

            var collection = schema.Components.OfType<ICollectionSchema>().FirstOrDefault();

            Assert.NotNull(collection);
            Assert.NotNull(collection.Item);
            Assert.Equal(typeof(IMyItem).ToTypeFullName(), collection.Item.SchemaId);
            Assert.Equal(typeof(IEnumerable<IMyItem>).ToTypeFullName(), collection.SchemaId);
            Assert.True(collection.Item.Properties.Any(x => x.Name == "Path" && x.PropertyType == typeof(string)));
        }

        [Fact]
        public void when_product_type_has_enumerable_property_then_collection_default_name_matches_property_name()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.False(schema.Properties.Any(x => x.Name == "MyElement"));

            var collection = schema.Components.OfType<ICollectionSchema>().FirstOrDefault();

            Assert.NotNull(collection);
            Assert.Equal("MyItems", collection.DefaultName);
        }

        [Fact]
        public void when_product_type_has_custom_enumerable_property_then_creates_collection_schema()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.False(schema.Properties.Any(x => x.Name == "MyCustomItems"));

            var collection = schema.Components.OfType<ICollectionSchema>()
                .FirstOrDefault(x => x.SchemaId.EndsWith("IMyItems"));

            Assert.NotNull(collection);
            Assert.NotNull(collection.Item);
            Assert.Equal(typeof(IMyItem).ToTypeFullName(), collection.Item.SchemaId);
            Assert.Equal(typeof(IMyItems).ToTypeFullName(), collection.SchemaId);
            Assert.True(collection.Properties
                .Any(x => x.Name == "IsSafe" && x.PropertyType == typeof(bool)));
            Assert.True(collection.Item.Properties
                .Any(x => x.Name == "Path" && x.PropertyType == typeof(string)));
        }

        [Fact]
        public void when_product_type_has_custom_enumerable_property_then_collection_default_name_matches_property_name()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyProduct));

            Assert.False(schema.Properties.Any(x => x.Name == "MyCustomItems"));

            var collection = schema.Components.OfType<ICollectionSchema>()
                .FirstOrDefault(x => x.SchemaId.EndsWith("IMyItems"));

            Assert.NotNull(collection);
            Assert.Equal("MyCustomItems", collection.DefaultName);
        }

        [Fact]
        public void when_product_has_name_property_then_it_is_not_added_as_dynamic_property()
        {
            var builder = new SchemaBuilder();

            var schema = builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyNamedProduct));

            Assert.Equal(1, schema.Properties.Count());

            Assert.True(schema.Properties.Any(x => x.Name == "IsPublic" && x.PropertyType == typeof(bool)));
        }

        [Fact]
        public void when_product_has_name_property_with_non_string_type_then_throws()
        {
            var builder = new SchemaBuilder();

            Assert.Throws<ArgumentException>(() => builder.BuildProduct(new ToolkitSchema("MyToolkit", "1.0"), typeof(IMyNamedWrongTypeProduct)));
        }
    }

    public interface IMyNamedProduct
    {
        string Name { get; set; }
        bool IsPublic { get; set; }
    }

    public interface IMyNamedWrongTypeProduct
    {
        Guid Name { get; set; }
    }

    public interface IMyProduct
    {
        string AccessKey { get; set; }
        bool IsPublic { get; set; }
        int Port { get; set; }
        IMyElement MyElement { get; set; }
        IEnumerable<IMyItem> MyItems { get; }
        IMyItems MyCustomItems { get; }
        IMyRenamedProperty Other { get; set; }
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

    public interface IMyRenamedProperty { }
}