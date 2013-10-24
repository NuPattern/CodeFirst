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
        public void when_creating_element_then_raises_component_added()
        {
            var product = new Product("Foo", "IFoo");
            var added = default(IComponent);

            product.ComponentAdded += (sender, args) => added = args.Value;

            var child = product.CreateElement("Storage", "IStorage");

            Assert.NotNull(added);
            Assert.Same(added, child);
        }

        [Fact]
        public void when_element_new_name_is_duplicated_with_other_element_then_throws()
        {
            var product = new Product("Foo", "IFoo");

            product.CreateElement("Storage", "IStorage");
            
            var child = product.CreateElement("Storage2", "IStorage");

            Assert.Throws<ArgumentException>(() => child.Name = "Storage");
        }

        [Fact]
        public void when_element_new_name_is_duplicated_with_container_property_then_throws()
        {
            var product = new Product("Foo", "IFoo");
            product.CreateProperty("Element");
            
            var child = product.CreateElement("Storage", "IStorage");

            Assert.Throws<ArgumentException>(() => child.Name = "Element");
        }

        [Fact]
        public void when_element_name_changed_then_raises_property_changing()
        {
            var product = new Product("Foo", "IFoo");
            var changed = false;

            product.PropertyChanging += (sender, args) =>
                {
                    changed = true;
                    Assert.Equal("Name", args.PropertyName);
                    Assert.Equal("Foo", args.OldValue);
                    Assert.Equal("Bar", args.NewValue);
                };

            product.Name = "Bar";

            Assert.True(changed);
        }

        [Fact]
        public void when_element_name_changed_then_raises_property_changed()
        {
            var product = new Product("Foo", "IFoo");
            var changed = false;

            product.PropertyChanged += (sender, args) =>
            {
                changed = true;
                Assert.Equal("Name", args.PropertyName);
                Assert.Equal("Foo", args.OldValue);
                Assert.Equal("Bar", args.NewValue);
            };

            product.Name = "Bar";

            Assert.True(changed);
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
        public void when_creating_component_named_as_property_then_throws()
        {
            var product = new Product("Foo", "IFoo");
            product.CreateProperty("Element");

            Assert.Throws<ArgumentException>(() => product.CreateElement("Element", "IElement"));
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
        public void when_deleting_element_then_raises_component_removed()
        {
            var product = new Product("Foo", "IFoo");
            var removed = default(IComponent);
            var child = product.CreateElement("Storage", "IStorage");

            product.ComponentRemoved += (sender, args) => removed = args.Value;

            child.Delete();

            Assert.NotNull(removed);
            Assert.Same(removed, child);
        }

        [Fact]
        public void when_deleting_collection_item_then_removes_from_parent_collection()
        {
            var product = new Product("Foo", "IFoo");
            var collection = product.CreateCollection("Collection", "ICollection");
            var child = collection.CreateItem("Item", "IItem");

            child.Delete();

            Assert.Equal(0, collection.Items.Count());
        }

        [Fact]
        public void when_deleting_collection_item_then_raises_item_deleted()
        {
            var product = new Product("Foo", "IFoo");
            var collection = product.CreateCollection("Collection", "ICollection");
            var child = collection.CreateItem("Item", "IItem");

            var deleted = default(IElement);
            collection.ItemRemoved += (sender, args) => deleted = args.Value;

            child.Delete();

            Assert.NotNull(deleted);
            Assert.Same(child, deleted);
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
        public void when_disposing_element_then_does_not_raise_deleted_event()
        {
            var product = new Product("Foo", "IFoo");
            var child = product.CreateElement("Storage", "IStorage");

            var deleted = false;
            child.Deleted += (s, e) => deleted = true;

            child.Dispose();

            Assert.False(deleted);
        }

        [Fact]
        public void when_disposing_element_then_does_not_remove_from_parent()
        {
            var product = new Product("Foo", "IFoo");
            var child = product.CreateElement("Storage", "IStorage");

            child.Dispose();

            Assert.Equal(1, product.Components.Count());
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
        public void when_creating_name_property_then_throws_because_its_intrinsic()
        {
            var product = new Product("Foo", "IFoo");

            Assert.Throws<ArgumentException>(() => product.CreateProperty("Name"));
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
        public void when_creating_collection_item_then_raises_item_added()
        {
            var product = new Product("Product", "IProduct");
            var collection = product.CreateCollection("Collection", "ICollection");

            var item = default(IElement);

            collection.ItemAdded += (sender, args) => item = args.Value;

            var created = collection.CreateItem("Item", "IItem");

            Assert.NotNull(item);
            Assert.Same(created, item);
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

        [Fact]
        public void when_property_changes_then_notifies_component()
        {
            var product = new Product("Product", "IProduct");
            product.CreateProperty("key").SetValue("foo");

            var changed = default(PropertyChangeEventArgs);

            product.PropertyChanged += (sender, args) => changed = args;

            product.Set("key", "bar");

            Assert.NotNull(changed);
            Assert.Equal("key", changed.PropertyName);
            Assert.Equal("foo", changed.OldValue);
            Assert.Equal("bar", changed.NewValue);
        }

        [Fact]
        public void when_property_changing_then_notifies_component()
        {
            var product = new Product("Product", "IProduct");
            product.CreateProperty("key").SetValue("foo");

            var changing = default(PropertyChangeEventArgs);

            product.PropertyChanging += (sender, args) => changing = args;

            product.Set("key", "bar");

            Assert.NotNull(changing);
            Assert.Equal("key", changing.PropertyName);
            Assert.Equal("foo", changing.OldValue);
            Assert.Equal("bar", changing.NewValue);
        }

        [Fact]
        public void when_property_set_to_same_existing_value_then_does_not_raise_propertychanged()
        {
            var product = new Product("Product", "IProduct");
            product.CreateProperty("key").SetValue("foo");

            var changed = default(PropertyChangeEventArgs);

            product.PropertyChanged += (sender, args) => changed = args;

            product.Set("key", "foo");

            Assert.Null(changed);
        }

        [Fact]
        public void when_property_changed_on_element_then_raises_property_changed_on_parent()
        {
            var product = new Product("Product", "IProduct");
            product.CreateElement("Element", "IElement")
                .CreateProperty("key").SetValue("foo");

            var changed = default(PropertyChangeEventArgs);

            product.PropertyChanged += (sender, args) => changed = args;

            product.Components.First().Set("key", "bar");

            Assert.NotNull(changed);
            Assert.Equal("Element", changed.PropertyName);
            Assert.Same(changed.OldValue, changed.NewValue);
            Assert.Same(product.Components.First(), changed.NewValue);
        }

        [Fact]
        public void when_property_changed_on_collection_item_then_raises_items_property_changed_on_collection()
        {
            var product = new Product("Product", "IProduct");
            product
                .CreateCollection("Collection", "ICollection")
                .CreateItem("Item", "IItem")
                .CreateProperty("key").SetValue("foo");

            var changed = default(PropertyChangeEventArgs);

            product.Components.First().PropertyChanged += (sender, args) => changed = args;

            product.Components.OfType<ICollection>().First().Items.First().Set("key", "bar");

            Assert.NotNull(changed);
            Assert.Equal("Items", changed.PropertyName);
            Assert.Same(changed.OldValue, changed.NewValue);
            Assert.Same(product.Components.OfType<ICollection>().First().Items.First(), changed.NewValue);
        }

        [Fact]
        public void when_property_changed_on_collection_item_then_raises_property_changed_on_parent()
        {
            var product = new Product("Product", "IProduct");
            product
                .CreateCollection("Collection", "ICollection")
                .CreateItem("Item", "IItem")
                .CreateProperty("key").SetValue("foo");

            var changed = default(PropertyChangeEventArgs);

            product.PropertyChanged += (sender, args) => changed = args;

            product.Components.OfType<ICollection>().First().Items.First().Set("key", "bar");

            Assert.NotNull(changed);
            Assert.Equal("Collection", changed.PropertyName);
            Assert.Same(changed.OldValue, changed.NewValue);
            Assert.Same(product.Components.First(), changed.NewValue);
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
                Components =
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
                Components =
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
                Properties = 
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
                Components =
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
                Components =
                {
                    new ElementSchema("IElement")
                    {
                        Properties = 
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
            Assert.Equal(typeof(bool), child.Properties.First().Schema.PropertyType);
        }
    }
}