namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using Xunit;

    public class ComponentFixture
    {
        [Fact]
        public void when_component_created_for_json_then_can_retrieve_component_from_json()
        {
            var json = new JObject();
            var component = new TestComponent(json);

            Assert.Same(component, json.AsComponent());
        }

        [Fact]
        public void when_component_has_property_then_name_matches_property()
        {
            var json = new JObject();
            var jprop = new JProperty("foo", json);
            var jparent = new JObject(jprop);
            var component = new TestComponent(json, jprop);

            Assert.Equal("foo", component.Name);
        }

        [Fact]
        public void when_component_has_property_then_setting_name_renames_property()
        {
            var json = new JObject();
            var jprop = new JProperty("foo", json);
            var jparent = new JObject(jprop);
            var component = new TestComponent(json, jprop);

            component.Name = "bar";

            Assert.Null(jparent.Property("foo"));
            Assert.NotNull(jparent.Property("bar"));
        }

        [Fact]
        public void when_new_component_name_already_exists_as_property_then_throws()
        {
            var json = new JObject();
            var jprop = new JProperty("foo", json);
            var jparent = new JObject(jprop, new JProperty("bar", new JObject()));
            var component = new TestComponent(json, jprop);

            Assert.Throws<ArgumentException>(() => component.Name = "bar");
        }

        [Fact]
        public void when_component_does_not_have_parent_property_then_can_rename()
        {
            // This is the component in a collection scenario.
            var json = new JObject();
            var component = new TestComponent(json);

            component.Name = "foo";

            Assert.Equal("foo", component.Name);

            component.Name = "bar";

            Assert.Equal("bar", component.Name);
        }

        [Fact]
        public void when_component_properties_retrieved_then_transient_internal_properties_are_not_exposed()
        {
            var json = new JObject(new JProperty("foo", "value"));
            var jprop = new JProperty("component", json);
            var jparent = new JObject(jprop);
            var component = new TestComponent(json, jprop);

            Assert.Equal(1, component.Properties.Count());
            Assert.Equal("foo", component.Properties.First().Name);
        }

        [Fact]
        public void when_component_property_retrieved_twice_then_gets_same_instance()
        {
            var json = new JObject(new JProperty("foo", "value"));
            var jprop = new JProperty("component", json);
            var jparent = new JObject(jprop);
            var component = new TestComponent(json, jprop);

            var prop = component.Properties.First();

            Assert.Equal("foo", prop.Name);
            Assert.Same(prop, component.Properties.First());
        }

        [Fact]
        public void when_component_property_deleted_then_removes_from_component_properties()
        {
            var json = new JObject(new JProperty("foo", "value"));
            var jprop = new JProperty("component", json);
            var jparent = new JObject(jprop);
            var component = new TestComponent(json, jprop);

            component.Properties.First().Delete();

            Assert.Equal(0, component.Properties.Count());
        }

        [Fact]
        public void when_component_property_deleted_then_re_added_then_gets_new_instance()
        {
            var json = new JObject(new JProperty("foo", "value"));
            var jprop = new JProperty("component", json);
            var jparent = new JObject(jprop);
            var component = new TestComponent(json, jprop);
            var prop = component.Properties.First();

            prop.Delete();

            json.Add(new JProperty("foo", "value2"));

            var prop2 = component.Properties.First();

            Assert.Equal("foo", prop2.Name);
            Assert.NotSame(prop, prop2);
        }

        [Fact]
        public void when_property_is_created_then_owner_is_component()
        {
            var json = new JObject();
            var component = new TestComponent(json);
            var property = component.CreateProperty("foo");

            Assert.Same(component, property.Owner);
        }

        [Fact]
        public void when_component_has_no_schema_then_created_property_has_no_schema()
        {
            var json = new JObject();
            var component = new TestComponent(json);
            var property = component.CreateProperty("foo");

            Assert.Null(property.Schema);
        }

        [Fact]
        public void when_component_has_schema_with_matching_property_then_created_property_has_schema()
        {
            var json = new JObject();
            var component = new TestComponent(json)
            {
                Schema = new ElementSchema("Test")
                {
                    Properties =
                    {
                        new PropertySchema("foo", typeof(string))
                    }
                },
            };
            var property = component.CreateProperty("foo");

            Assert.NotNull(property.Schema);
        }

        [Fact]
        public void when_second_property_is_created_then_properties_are_sorted_by_name()
        {
            var json = new JObject();
            var component = new TestComponent(json);
            component.EndInit();

            component.CreateProperty("foo");
            component.CreateProperty("bar");

            Assert.Equal("bar", component.Properties.First().Name);
        }

        [Fact]
        public void when_begin_init_called_then_properties_are_not_sorted_until_end_init()
        {
            var json = new JObject();
            var component = new TestComponent(json);

            component.BeginInit();
            component.CreateProperty("foo");
            component.CreateProperty("bar");

            Assert.Equal("foo", component.Properties.First().Name);

            component.EndInit();

            Assert.Equal("bar", component.Properties.First().Name);
        }

        private class TestComponent : Component
        {
            public TestComponent(JObject component)
                : base(component)
            {
            }

            public TestComponent(JObject component, JProperty property)
                : base(component, property)
            {
            }
        }
    }
}