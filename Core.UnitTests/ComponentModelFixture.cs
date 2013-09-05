namespace NuPattern.ComponentModelFixture
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using Xunit;

    public class given_a_product
    {
        [Fact]
        public void when_creating_element_then_references_parent_product()
        {
            var product = new Product("Foo", "IFoo");

            var child = product.CreateElement("Storage", "IStorage");

            Assert.Same(child.Parent, product);
        }

        [Fact]
        public void when_creating_collection_then_references_parent_product()
        {
            var product = new Product("Foo", "IFoo");

            var child = product.CreateCollection("Buckets", "IBuckets");

            Assert.Same(child.Parent, product);
        }

        [Fact]
        public void when_creating_element_then_sets_name_and_schema_id()
        {
            var product = new Product("Foo", "IFoo");

            var child = product.CreateElement("Storage", "IStorage");

            Assert.Equal("Storage", child.Name);
            Assert.Equal("IStorage", child.SchemaId);
        }

        [Fact]
        public void when_creating_collection_then_sets_name_and_schema_id()
        {
            var product = new Product("Foo", "IFoo");

            var child = product.CreateCollection("Buckets", "IBuckets");

            Assert.Equal("Buckets", child.Name);
            Assert.Equal("IBuckets", child.SchemaId);
        }

        [Fact]
        public void when_enumerating_components_then_can_access_created_element()
        {
            var product = new Product("Foo", "IFoo");
            var child = product.CreateElement("Storage", "IStorage");

            var component = product.Components.FirstOrDefault();

            Assert.NotNull(component);
            Assert.Same(child, component);
        }

        [Fact]
        public void when_enumerating_components_then_can_access_created_collection()
        {
            var product = new Product("Foo", "IFoo");
            var child = product.CreateCollection("Buckets", "IBuckets");

            var component = product.Components.FirstOrDefault();

            Assert.NotNull(component);
            Assert.Same(child, component);
        }

        [Fact]
        public void when_creating_duplicate_name_component_then_throws()
        {
            var product = new Product("Foo", "IFoo");
            var child = product.CreateCollection("Storage", "IStorage");

            // Same name and schema id
            Assert.Throws<ArgumentException>(() => product.CreateCollection("Storage", "IStorage"));
            // Same name, different schema id
            Assert.Throws<ArgumentException>(() => product.CreateCollection("Storage", "IBucket"));

            // Different component type, same name and schema id
            Assert.Throws<ArgumentException>(() => product.CreateElement("Storage", "IStorage"));
            // Different component type, same name, different schema id
            Assert.Throws<ArgumentException>(() => product.CreateElement("Storage", "IBucket"));
        }

        [Fact]
        public void when_deleting_element_then_removes_from_parent_components()
        {
            var product = new Product("Foo", "IFoo");
            var child = product.CreateElement("Storage", "IStorage");

            child.Delete();

            Assert.Equal(0, product.Components.Count());
        }

        [Fact]
        public void when_deleting_element_then_disposes_it()
        {
            var product = new Product("Foo", "IFoo");
            var child = product.CreateElement("Storage", "IStorage");

            child.Delete();

            Assert.True(child.IsDisposed);
        }

        [Fact]
        public void when_deleting_renamed_element_then_removes_from_parent_components()
        {
            var product = new Product("Foo", "IFoo");
            var child = product.CreateElement("Storage", "IStorage");

            child.Name = "Bar";
            child.Delete();

            Assert.Equal(0, product.Components.Count());
        }

        [Fact]
        public void when_creating_property_then_owner_is_parent()
        {
            var product = new Product("Foo", "IFoo");

            var prop = product.CreateProperty("IsPublic");

            Assert.Same(product, prop.Owner);
        }

        [Fact]
        public void when_creating_duplicate_property_then_throws()
        {
            var product = new Product("Foo", "IFoo");
            var prop = product.CreateProperty("IsPublic");

            Assert.Throws<ArgumentException>(() => product.CreateProperty("IsPublic"));
        }

        [Fact]
        public void when_creating_property_then_owner_properties_contains_new_property()
        {
            var product = new Product("Foo", "IFoo");

            var prop = product.CreateProperty("IsPublic");

            Assert.Same(prop, product.Properties.FirstOrDefault());
        }

        [Fact]
        public void when_creating_hierarchy_then_can_access_owning_product()
        {
            var product = new Product("Foo", "IFoo");
            var child = product.CreateElement("Bar", "IBar").CreateElement("Baz", "IBaz");

            Assert.Same(product, child.Product);
        }

        [Fact]
        public void when_setting_property_then_automatically_creates_it_if_missing()
        {
            var product = new Product("Foo", "IFoo");

            product.Set("IsPublic", true);

            Assert.True(product.Get<bool>("IsPublic"));
            Assert.NotNull(product.Properties.FirstOrDefault());
            Assert.Equal("IsPublic", product.Properties.First().Name);
        }

        [Fact]
        public void when_getting_property_then_does_not_create_if_missing()
        {
            var product = new Product("Foo", "IFoo");

            Assert.False(product.Get<bool>("IsPublic"));
            Assert.Equal(0, product.Properties.Count());
        }

        [Fact]
        public void when_deleting_property_then_removes_from_owner()
        {
            var product = new Product("Foo", "IFoo");
            var prop = product.CreateProperty("IsPublic");

            prop.Delete();

            Assert.Equal(0, product.Properties.Count());
        }

        [Fact]
        public void when_deleting_property_then_owner_becomes_null()
        {
            var product = new Product("Foo", "IFoo");
            var prop = product.CreateProperty("IsPublic");

            prop.Delete();

            Assert.Null(prop.Owner);
        }

        [Fact]
        public void when_creating_collection_item_then_can_access_it()
        {
            var product = new Product("Product", "IProduct");
            var collection = product.CreateCollection("Collection", "ICollection");
            var item = collection.CreateItem("Item", "IItem");

            Assert.Equal(1, collection.Items.Count());
            Assert.Same(item, collection.Items.First());
        }

        [Fact]
        public void when_creating_collection_item_then_can_access_product()
        {
            var product = new Product("Product", "IProduct");
            var collection = product.CreateCollection("Collection", "ICollection");
            var item = collection.CreateItem("Item", "IItem");

            Assert.Same(product, item.Product);
        }

        [Fact]
        public void when_creating_duplicate_name_collection_item_then_throws()
        {
            var product = new Product("Product", "IProduct");
            var collection = product.CreateCollection("Collection", "ICollection");
            var item = collection.CreateItem("Item", "IItem");

            Assert.Throws<ArgumentException>(() => collection.CreateItem("Item", "IItem"));
        }

        [Fact]
        public void when_disposing_product_then_disposes_entire_graph()
        {
            var product = new Product("Product", "IProduct");
            product.CreateCollection("Collection", "ICollection")
                .CreateItem("Item", "IItem");
            product.CreateElement("Element", "IElement")
                .CreateElement("NestedElement", "IElement");

            product.Dispose();

            Assert.True(product.IsDisposed);
            Assert.True(product.Components.All(c => c.IsDisposed));
            Assert.True(product.Components.OfType<Collection>().All(c => c.Items.All(e => e.IsDisposed)));
        }
    }

    public class given_a_product_with_schema
    {
        [Fact]
        public void when_creating_element_then_it_has_schema()
        {
            var product = new Product("Foo", "IFoo");
            product.Schema = new ProductSchema("IFoo")
            {
                ComponentSchemas =
                {
                    new ElementSchema("IElement"),
                }
            };

            var child = product.CreateElement("Element", "IElement");

            Assert.NotNull(child.Schema);
        }

        [Fact]
        public void when_creating_collection_then_it_has_schema()
        {
            var product = new Product("Foo", "IFoo");
            product.Schema = new ProductSchema("IFoo")
            {
                ComponentSchemas =
                {
                    new CollectionSchema("ICollection")
                    {
                        ItemSchema = new ElementSchema("IElement")
                    }
                }
            };

            var child = product.CreateCollection("Buckets", "ICollection");

            Assert.NotNull(child.Schema);
        }

        [Fact]
        public void when_creating_property_then_it_has_schema()
        {
            var product = new Product("Foo", "IFoo");
            product.Schema = new ProductSchema("IFoo")
            {
                PropertySchemas = 
                {
                    new PropertySchema("IsPublic", typeof(bool))
                }
            };

            var prop = product.CreateProperty("IsPublic");

            Assert.NotNull(prop.Schema);
        }

        [Fact]
        public void when_creating_collection_item_then_it_has_schema()
        {
            var product = new Product("Product", "IProduct");
            product.Schema = new ProductSchema("IFoo")
            {
                ComponentSchemas =
                {
                    new CollectionSchema("ICollection")
                    {
                        ItemSchema = new ElementSchema("IElement")
                    }
                }
            };
            
            var collection = product.CreateCollection("Collection", "ICollection");
            var item = collection.CreateItem("Item", "IElement");

            Assert.NotNull(item.Schema);
        }

        [Fact]
        public void when_creating_element_then_it_has_schema_defined_properties()
        {
            var product = new Product("Foo", "IFoo");
            product.Schema = new ProductSchema("IFoo")
            {
                ComponentSchemas =
                {
                    new ElementSchema("IElement")
                    {
                        PropertySchemas = 
                        {
                            new PropertySchema("IsPublic", typeof(bool))
                        }
                    },
                }
            };

            var child = product.CreateElement("Element", "IElement");

            Assert.Equal(1, child.Properties.Count());
            Assert.Equal("IsPublic", child.Properties.First().Name);
            Assert.NotNull(child.Properties.First().Schema);
            Assert.Equal(typeof(bool), child.Properties.First().Schema.Type);
        }
    }
}