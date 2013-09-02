namespace NuPattern
{
    using Moq;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using Xunit;

    public class InstanceVisitorFixture
    {
        [Fact]
        public void when_visiting_root_then_visits_all_delegates()
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

            var product = Mock.Of<IProduct>(p => 
                p.Components == new IComponent[] { 
                    Mock.Of<IElement>(e => e.Properties == new [] 
                    {
                        Mock.Of<IProperty>()
                    } &&
                    e.Accept<InstanceVisitor>(It.IsAny<InstanceVisitor>()) == visitor.VisitElement(e)),
                    Mock.Of<ICollection>(c => c.Items == new [] 
                    {
                        Mock.Of<IElement>(e => e.Accept<InstanceVisitor>(It.IsAny<InstanceVisitor>()) == visitor.VisitElement(e)),
                    } &&
                    c.Accept<InstanceVisitor>(It.IsAny<InstanceVisitor>()) == visitor.VisitCollection(c)),
                } && 
                p.Properties == new [] 
                {
                    Mock.Of<IProperty>()
                });

            visitor.VisitProduct(product);

            Assert.Equal(1, products);
            Assert.Equal(2, elements);
            Assert.Equal(1, collections);
            Assert.Equal(4, containers);
            Assert.Equal(4, components);
            Assert.Equal(2, properties);
        }
    }
}