namespace NuPattern
{
    using Moq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;

    public class ModelFixture
    {
        [Fact]
        public void when_reading_json_then_can_build_component_layer()
        {
            var json = (JObject)JsonConvert.DeserializeObject(File.ReadAllText("ComponentModel.json"));
            IProduct product = new Product(json);

            Assert.Equal("NuPattern", product.Name);
            Assert.Equal("AWS", product.Toolkit.Id);
            Assert.Equal("1.0.0", product.Toolkit.Version);
            Assert.Equal("NuPattern.Toolkit", product.SchemaId);
            Assert.True((bool)product.Properties.First(x => x.Name == "IsCool").Value);
            Assert.Equal(10, (long)product.Properties.First(x => x.Name == "Count").Value);

            Assert.Equal(2, product.Components.Count());

            Assert.Equal("ComponentA", product.Components.First().Name);
            Assert.Equal("Element", product.Components.First().SchemaId);
            Assert.Equal("something", product.Components.First().Properties.First(x => x.Name == "Something").Value);

            Assert.Equal("ComponentB", product.Components.Skip(1).First().Name);
            Assert.Equal("Collection", product.Components.Skip(1).First().SchemaId);
            Assert.Equal("something", product.Components.Skip(1).First().Properties.First(x => x.Name == "Something").Value);

            Assert.Same(product.Components.First(), product.Components.First());

            product.Name = "test";

            //Console.WriteLine(json);
        }

        [Fact]
        public void when_reading_json_then_toolkit_element_is_not_component()
        {
            var json = (JObject)JsonConvert.DeserializeObject(File.ReadAllText("Model.json"));
            IProduct product = new Product(json);

            Assert.Equal(1, product.Components.Count());
        }

        [Fact]
        public void when_reading_json_then_can_build_collections_with_properties()
        {
            var json = (JObject)JsonConvert.DeserializeObject(File.ReadAllText("Model.json"));
            IProduct product = new Product(json);

            Assert.Equal("Amazon", product.Name);
            Assert.Equal("AWS", product.Toolkit.Id);
            Assert.Equal("1.0.0", product.Toolkit.Version);
            Assert.Equal("asdf", (string)product.Properties.First(x => x.Name == "AccessKey").Value);

            Assert.Equal(1, product.Components.Count());

            Assert.Equal("Storage", product.Components.First().Name);
            Assert.Equal("NuPattern.Toolkit.Simple.IStorage", product.Components.First().SchemaId);
            Assert.Equal(true, (bool)product.Components.First().Properties.First(x => x.Name == "RefreshOnLoad").Value);

            Assert.Equal(true, product.Components.OfType<IElement>()
                .First().Components.First(c => c.Name == "Buckets")
                .Properties.First(p => p.Name == "RefreshOnLoad").Value);
        }

        [Fact]
        public void when_converting_to_model_twice_then_retrieves_same_instance()
        {
            var json = (JObject)JsonConvert.DeserializeObject(File.ReadAllText("ComponentModel.json"));
            IProduct product = new Product(json);

            Assert.Same(product.Components.First(), product.Components.First());
        }

        [Fact]
        public void when_accessing_extension_then_retrieves_as_product()
        {
            var json = (JObject)JsonConvert.DeserializeObject(File.ReadAllText("ComponentModel.json"));
            IProduct product = new Product(json);

            var extension = product.Extensions.First();

            Assert.NotNull(extension);
            Assert.Equal("ExtensionA", extension.Name);
            Assert.Equal("bar", extension.Properties.First(x => x.Name == "Foo").Value);
            Assert.Same(product, extension.Parent);
        }

        [Fact]
        public void when_changing_property_value_then_raises_property_events()
        {
            var json = (JObject)JsonConvert.DeserializeObject(File.ReadAllText("ComponentModel.json"));
            var product = (IProduct)new Product(json);

            var changing = "";
            var changed = "";

            product.PropertyChanging += (sender, args) => changing = args.PropertyName;
            product.PropertyChanged += (sender, args) => changed = args.PropertyName;

            product.Name = "test";

            Assert.Equal("Name", changing);
            Assert.Equal("Name", changed);
        }

        [Fact]
        public void when_creating_product_then_can_add_properties()
        {
            var json = new JObject();
            var product = new Product(json)
            {
                Name = "Amazon",
            };

            product.Set("Definition", "IAmazonWebServices");
            product.Set("AccessKey", "asdf");

            var storage = product.CreateElement("Storage", "IStorage");
            storage.Set("RefreshOnLoad", true);
            var buckets = (Collection)storage.CreateCollection("Buckets");
            buckets.Set("RefreshOnLoad", true);
            buckets.Schema = Mock.Of<ICollectionSchema>(c => c.ItemSchema == "IBucket");

            var foo = buckets.CreateItem("foo").Set("Permissions", PlatformID.Win32NT);
            var bar = buckets.CreateItem("bar").Set("Permissions", PlatformID.MacOSX);

            Assert.Equal("asdf", product.Get<string>("AccessKey"));
            Assert.Equal(true, storage.Get<bool>("RefreshOnLoad"));
            Assert.Equal(true, buckets.Get<bool>("RefreshOnLoad"));
            Assert.Equal(PlatformID.Win32NT, foo.Get<PlatformID>("Permissions"));
            Assert.Equal(PlatformID.MacOSX, bar.Get<PlatformID>("Permissions"));

            //Console.WriteLine(json);
        }

        public void Do()
        {
            var value = Get(() => this.Id);

        }

        private object Get<T>(Expression<Func<T>> prop)
        {
            return null;
        }

        public int Id { get; set; }
    }
}