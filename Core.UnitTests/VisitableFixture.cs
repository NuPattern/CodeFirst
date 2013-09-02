namespace NuPattern
{
    using System;
    using System.Linq;
    using Xunit;

    public class VisitableFixture
    {
        [Fact]
        public void when_visiting_root_then_visits_entire_graph()
        {
            var products = 0;
            var elements = 0;
            var collections = 0;
            var containers = 0;
            var components = 0;
            var properties = 0;

            var visitor = InstanceVisitor.Create(
                p => products++,
                e => elements++,
                c => collections++,
                c => containers++,
                c => components++,
                p => properties++);


            var product = new Product("Foo", "IFoo");
            product.CreateElement("Element", "IElement")
                .CreateProperty("IsVisible");
            product.CreateCollection("Collection", "ICollection")
                .CreateElement("Element", "IElement");
            product.CreateProperty("IsVisible");

            product.Accept(visitor);

            Assert.Equal(1, products);
            Assert.Equal(2, elements);
            Assert.Equal(1, collections);
            Assert.Equal(4, containers);
            Assert.Equal(4, components);
            Assert.Equal(2, properties);
        }
    }
}