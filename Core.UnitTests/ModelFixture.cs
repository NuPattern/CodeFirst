namespace NuPattern
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
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
            Assert.Equal("NuPattern.Toolkit", product.Definition);
            Assert.True((bool)product.Properties.First(x => x.Definition == "IsCool").Value);
            Assert.Equal(10, (long)product.Properties.First(x => x.Definition == "Count").Value);

            Assert.Equal(2, product.Components.Count());

            Assert.Equal("ComponentA", product.Components.First().Name);
            Assert.Equal("Element", product.Components.First().Definition);
            Assert.Equal("something", product.Components.First().Properties.First(x => x.Definition == "Something").Value);

            Assert.Equal("ComponentB", product.Components.Skip(1).First().Name);
            Assert.Equal("Collection", product.Components.Skip(1).First().Definition);
            Assert.Equal("something", product.Components.Skip(1).First().Properties.First(x => x.Definition == "Something").Value);

            Assert.Same(product.Components.First(), product.Components.First());

            product.Name = "test";

            //Console.WriteLine(json);
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
            Assert.Equal("bar", extension.Properties.First(x => x.Definition == "Foo").Value);
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