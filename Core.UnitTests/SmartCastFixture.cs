namespace NuPattern.SmartCastFixture
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using NetFx.StringlyTyped;

    public class given_a_toolkit
    {
        [Fact]
        public void when_smart_casting_to_concrete_type_then_throws()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();

            Assert.Throws<ArgumentException>(() => cast.Cast(product, typeof(given_a_toolkit)));
        }

        [Fact]
        public void when_smart_casting_product_without_schema_then_throws()
        {
            var product = new Product("MyProduct", "1.0");
            var cast = new SmartCast();

            Assert.Throws<ArgumentException>(() => cast.Cast(product, typeof(IMyProduct)));
        }

        [Fact]
        public void when_smart_casting_then_succeeds()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();

            var proxy = (IMyProduct)cast.Cast(product, typeof(IMyProduct));

            Assert.NotNull(proxy);
            Assert.Equal("asdf", proxy.Key);
            Assert.Equal(product.Name, proxy.Name);
        }

        [Fact]
        public void when_smart_casting_to_derived_then_can_access_base_properties()
        {
            var product = new Product("MyProduct", "IMyProduct");
            product.CreateProperty("Key").Value = "asdf";
            product.CreateProperty("IsVisible").Value = true;
            ComponentMapper.SyncProduct(product, new ProductSchema("IMyProduct")
            {
                PropertySchemas =
                {
                    new PropertySchema("Key", typeof(string)),
                    new PropertySchema("IsVisible", typeof(bool)),
                },
                ToolkitSchema = new ToolkitSchema("MyToolkit", "1.0")
            });

            var cast = new SmartCast();

            var proxy = (IMyDerivedProduct)cast.Cast(product, typeof(IMyDerivedProduct));

            Assert.NotNull(proxy);
            Assert.Equal("asdf", proxy.Key);
            Assert.Equal(true, proxy.IsVisible);
            Assert.Equal(product.Name, proxy.Name);
        }

        [Fact]
        public void when_converting_proxy_to_component_then_can_access_its_members()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();
            var proxy = (IMyProduct)cast.Cast(product, typeof(IMyProduct));

            var component = proxy as IProduct;

            Assert.NotNull(component);
            Assert.NotNull(component.CreateProperty("OneMore"));
        }

        [Fact]
        public void when_converting_to_interface_with_extra_property_then_returns_null()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();

            var proxy = (IProductWithAdditionalProperty)cast.Cast(product, typeof(IProductWithAdditionalProperty));

            Assert.Null(proxy);
        }

        [Fact]
        public void when_casting_twice__then_gets_same_proxy_instance()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();

            var proxy1 = (IMyProduct)cast.Cast(product, typeof(IMyProduct));
            var proxy2 = (IMyProduct)cast.Cast(product, typeof(IMyProduct));

            Assert.Same(proxy1, proxy2);
        }

        [Fact]
        public void when_converting_to_interface_with_incompatible_property_then_returns_null()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();

            var proxy = (IProductWithIncompatibleProperty)cast.Cast(product, typeof(IProductWithIncompatibleProperty));

            Assert.Null(proxy);
        }

        [Fact]
        public void when_setting_property_via_proxy_then_sets_underlying_product_property()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();
            var proxy = (IMyProduct)cast.Cast(product, typeof(IMyProduct));

            proxy.Key = "newkey";

            Assert.Equal("newkey", proxy.Key);
            Assert.Equal("newkey", (string)product.Properties.First(x => x.Name == "Key").Value);
        }

        [Fact]
        public void when_setting_name_property_via_proxy_then_renames_component()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();
            var proxy = (IMyProduct)cast.Cast(product, typeof(IMyProduct));

            proxy.Name = "FooProduct";

            Assert.Equal("FooProduct", product.Name);
        }

        [Fact]
        public void when_getting_element_reference_then_gets_proxy()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();
            var proxy = (IMyProduct)cast.Cast(product, typeof(IMyProduct));

            var element = proxy.MyElement;

            Assert.Equal(8080, element.Port);
        }

        [Fact]
        public void when_getting_collection_reference_then_gets_proxy()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();
            var proxy = (IMyProduct)cast.Cast(product, typeof(IMyProduct));

            var collection = proxy.MyItems;

            Assert.Equal(2, collection.Count());
            Assert.Equal("a", collection.First().Tag);
            Assert.Equal("b", collection.Skip(1).First().Tag);
        }

        [Fact]
        public void when_getting_custom_collection_reference_then_gets_proxy()
        {
            var product = InitializeMyProduct();
            var cast = new SmartCast();
            var proxy = (IMyProductWithCustomCollection)cast.Cast(product, typeof(IMyProductWithCustomCollection));

            var collection = proxy.MyItems;

            Assert.Equal("forall", collection.PropForAll);
            Assert.Equal(2, collection.Count());
            Assert.Equal("a", collection.First().Tag);
            Assert.Equal("b", collection.Skip(1).First().Tag);
        }

        private static Product InitializeMyProduct()
        {
            var product = new Product("MyProduct", "IMyProduct");
            product.CreateProperty("Key").Value = "asdf";
            product.CreateElement("MyElement", "IMyElement")
                .CreateProperty("Port").Value = 8080;

            var collection = product.CreateCollection("MyItems", "IMyItems");
            collection.CreateItem("Foo", "IMyItem").CreateProperty("Tag").Value = "a";
            collection.CreateItem("Bar", "IMyItem").CreateProperty("Tag").Value = "b";
            collection.CreateProperty("PropForAll").Value = "forall";

            ComponentMapper.SyncProduct(product, new ProductSchema("IMyProduct")
            {
                PropertySchemas =
                {
                    new PropertySchema("Key", typeof(string)),
                },
                ComponentSchemas = 
                {
                    new ElementSchema("IMyElement")
                    {
                        PropertySchemas = 
                        {
                            new PropertySchema("Port", typeof(int)),
                        }
                    }, 
                    new CollectionSchema("IMyItems")
                    {
                        PropertySchemas = 
                        {
                            new PropertySchema("PropForAll", typeof(string)),
                        },
                        ItemSchema = new ElementSchema("IMyItem")
                        {
                            PropertySchemas = 
                            {
                                new PropertySchema("Tag", typeof(string)),
                            }
                        }
                    }
                },
                ToolkitSchema = new ToolkitSchema("MyToolkit", "1.0")
            });
            return product;
        }
    }

    public class given_a_type_with_extra_reference_property
    {
        private IProduct product;
        private SmartCast cast = new SmartCast();

        public given_a_type_with_extra_reference_property()
        {
            var product = new Product("MyProduct", "IMyProduct");
            ComponentMapper.SyncProduct(product, new ProductSchema("IMyProduct")
            {
                ToolkitSchema = new ToolkitSchema("MyToolkit", "1.0")
            });

            this.product = product;
        }

        [Fact]
        public void when_casting_product_without_element_property_then_returns_null()
        {
            var casted = cast.Cast(product, typeof(IMyProduct));

            Assert.Null(casted);
        }

        public interface IMyProduct
        {
            IMyElement Element { get; }
        }

        public interface IMyElement { }
    }

    public class given_a_type_with_incompatible_collection_item_type
    {
        private IProduct product;
        private SmartCast cast = new SmartCast();

        public given_a_type_with_incompatible_collection_item_type()
        {
            var product = new Product("MyProduct", "IMyProduct");
            ComponentMapper.SyncProduct(product, new ProductSchema("IMyProduct")
            {
                ComponentSchemas = 
                {
                    new CollectionSchema("IMyElements")
                    {
                        DefaultName = "MyElements",
                        ItemSchema = new ElementSchema("IMyElement")
                        {
                            PropertySchemas = 
                            {
                                new PropertySchema("Key", typeof(string)),
                            }
                        }
                    }
                },
                ToolkitSchema = new ToolkitSchema("MyToolkit", "1.0")
            });

            this.product = product;
        }

        [Fact]
        public void when_casting_product_then_returns_null()
        {
            var casted = cast.Cast(product, typeof(IMyProduct));

            Assert.Null(casted);
        }

        public interface IMyProduct
        {
            IEnumerable<IMyElement> MyElements { get; }
        }

        public interface IMyElement
        {
            int Key { get; set; }
        }
    }

    public class given_a_type_with_compatible_collection_item_type
    {
        private IProduct product;
        private SmartCast cast = new SmartCast();

        public given_a_type_with_compatible_collection_item_type()
        {
            var product = new Product("MyProduct", "IMyProduct");
            ComponentMapper.SyncProduct(product, new ProductSchema("IMyProduct")
            {
                ComponentSchemas = 
                {
                    new CollectionSchema("IMyElements")
                    {
                        DefaultName = "MyElements",
                        ItemSchema = new ElementSchema("IMyElement")
                        {
                            PropertySchemas = 
                            {
                                new PropertySchema("Key", typeof(string)),
                            }
                        }
                    }
                },
                ToolkitSchema = new ToolkitSchema("MyToolkit", "1.0")
            });

            this.product = product;
        }

        [Fact]
        public void when_casting_product_then_returns_valid_proxy()
        {
            var casted = cast.Cast(product, typeof(IFoo));

            Assert.NotNull(casted);
        }

        public interface IFoo
        {
            IEnumerable<IBar> MyElements { get; }
        }

        public interface IBar
        {
            string Key { get; set; }
        }
    }

    public class given_a_type_with_collection_item_type_for_product_with_element
    {
        private IProduct product;
        private SmartCast cast = new SmartCast();

        public given_a_type_with_collection_item_type_for_product_with_element()
        {
            var product = new Product("MyProduct", "IMyProduct");
            ComponentMapper.SyncProduct(product, new ProductSchema("IMyProduct")
            {
                ComponentSchemas = 
                {
                    new ElementSchema("IMyElement")
                    {
                        DefaultName = "MyElement",
                        PropertySchemas = 
                        {
                            new PropertySchema("Key", typeof(string)),
                        }
                    }
                },
                ToolkitSchema = new ToolkitSchema("MyToolkit", "1.0")
            });

            this.product = product;
        }

        [Fact]
        public void when_casting_product_then_returns_null()
        {
            var casted = cast.Cast(product, typeof(IFoo));

            Assert.Null(casted);
        }

        public interface IFoo
        {
            IBars MyElement { get; }
        }

        public interface IBars : IEnumerable<IBar> 
        {
            string Key { get; set; }
        }

        public interface IBar
        {
        }
    }

    public interface IMyProduct
    {
        string Name { get; set; }
        string Key { get; set; }
        IMyElement MyElement { get; }
        IEnumerable<IMyItem> MyItems { get; }
    }

    public interface IMyElement
    {
        int Port { get; set; }
    }

    public interface IMyItem
    {
        string Tag { get; set; }
    }

    public interface IMyProductWithCustomCollection
    {
        IMyItems MyItems { get; }
    }

    public interface IMyItems : IEnumerable<IMyItem>
    {
        string PropForAll { get; }
    }

    public interface IProductWithIncompatibleProperty
    {
        int Key { get; set; }
    }

    public interface IProductWithAdditionalProperty
    {
        int Port { get; set; }
    }

    public interface IMyDerivedProduct : IMyProduct
    {
        bool IsVisible { get; set; }
    }
}