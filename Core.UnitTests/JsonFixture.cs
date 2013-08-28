namespace NuPattern
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Xunit;

    public class JsonFixture
    {
        [Fact]
        public void when_retrieving_generic_property_then_converts_bool()
        {
            var now = DateTimeOffset.Now;
            var json = new JObject(
                new JProperty("IsGreat", true),
                new JProperty("Count", 10),
                new JProperty("When", now));

            Assert.Equal(true, json.Get(() => this.IsGreat));
            Assert.Equal(10, json.Get(() => this.Count));
            Assert.Equal(now, json.Get(() => this.When));
        }

        [Fact]
        public void when_retrieving_unsupported_json_type_then_throws()
        {
            var json = new JObject();

            Assert.Throws<ArgumentException>(() => json.Get(() => Uri));
        }

        [Fact]
        public void when_retrieving_value_from_non_existent_property_then_succeeds()
        {
            var json = new JObject();

            Assert.False(json.Get(() => IsGreat));
            Assert.Equal(null, json.Get(() => Name));
        }

        [Fact]
        public void when_setting_non_existing_property_then_adds_it()
        {
            var json = new JObject();

            json.Set(() => Name, "kzu");

            Assert.Equal("kzu", json.Get(() => Name));
        }

        [Fact]
        public void when_deserializing_model_then_can_traverse_all_jobjects()
        {
            var json = File.ReadAllText("ComponentModel.json");
            var obj = (JArray)JsonConvert.DeserializeObject(json);

            var objs = obj.Descendants().OfType<JObject>().Reverse().ToList();

            Assert.Equal(7, objs.Count);
        }

        public class JsonModel : JObject { }

        //[Fact]
        public void when_action_then_assert()
        {
            var json = new JObject();

            Render(() => json.GetValue("foo") == null
                ? default(string) : (string)json.GetValue("foo"));
        }

        private void Render<T>(Expression<Func<T>> expression)
        {
            Console.WriteLine(expression);
        }

        public string Name { get; set; }
        public bool IsGreat { get; set; }
        public int Count { get; set; }
        public DateTimeOffset When { get; set; }
        public UriBuilder Uri { get; set; }
    }
}