namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.IO;
    using System.Linq;
    using Xunit;

    public class ProductStoreFixture
    {
        [Fact]
        public void when_creating_product_then_sets_schema()
        {
            var store = new ProductStore(Path.GetTempFileName(), new[] 
            {
                new ToolkitSchema("Aws", "1.0")
                {
                    ProductSchemas =
                    {
                        new ProductSchema("IAws")
                    }
                }
            });

            var product = store.CreateProduct("Web", "IAws");

            Assert.NotNull(product);
            Assert.NotNull(product.Schema);
        }
    }
}