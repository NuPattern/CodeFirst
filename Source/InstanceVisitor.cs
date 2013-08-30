namespace NuPattern
{
    using System;
    using System.Linq;

    /// <summary>
    /// Base class for visitors of the runtime model.
    /// </summary>
    public abstract class InstanceVisitor
    {
        public static InstanceVisitor Create(
            Action<IProduct> visitProduct = null,
            Action<IElement> visitElement = null,
            Action<ICollection> visitCollection = null,
            Action<IContainer> visitContainer = null,
            Action<IComponent> visitComponent = null,
            Action<IProperty> visitProperty = null)
        {
            return new AnonymousInstanceVisitor
            {
                ProductVisitor = visitProduct,
                ElementVisitor = visitElement,
                CollectionVisitor = visitCollection,
                ContainerVisitor = visitContainer,
                ComponentVisitor= visitComponent,
                PropertyVisitor = visitProperty,
            };
        }

        public virtual InstanceVisitor VisitProduct(IProduct product)
        {
            VisitContainer(product);
            return this;
        }

        public virtual InstanceVisitor VisitElement(IElement element)
        {
            VisitContainer(element);
            return this;
        }

        public virtual InstanceVisitor VisitCollection(ICollection collection)
        {
            VisitContainer(collection);
            foreach (var item in collection.Items)
            {
                item.Accept(this);
            }
            return this;
        }

        protected virtual void VisitContainer(IContainer container)
        {
            VisitComponent(container);
            foreach (var component in container.Components)
            {
                component.Accept(this);
            }
        }

        protected virtual void VisitComponent(IComponent component)
        {
            foreach (var property in component.Properties)
            {
                VisitProperty(property);
            }
        }

        protected virtual void VisitProperty(IProperty property)
        {
        }

        private class AnonymousInstanceVisitor : InstanceVisitor
        {
            public Action<IProduct> ProductVisitor { get; set; }
            public Action<IElement> ElementVisitor { get; set; }
            public Action<ICollection> CollectionVisitor { get; set; }
            public Action<IContainer> ContainerVisitor { get; set; }
            public Action<IComponent> ComponentVisitor { get; set; }
            public Action<IProperty> PropertyVisitor { get; set; }

            public override InstanceVisitor VisitProduct(IProduct product)
            {
                if (ProductVisitor != null)
                    ProductVisitor(product);

                return base.VisitProduct(product);
            }

            public override InstanceVisitor VisitElement(IElement element)
            {
                if (ElementVisitor != null)
                    ElementVisitor(element);

                return base.VisitElement(element);
            }

            public override InstanceVisitor VisitCollection(ICollection collection)
            {
                if (CollectionVisitor != null)
                    CollectionVisitor(collection);

                return base.VisitCollection(collection);
            }

            protected override void VisitContainer(IContainer container)
            {
                if (ContainerVisitor != null)
                    ContainerVisitor(container);

                base.VisitContainer(container);
            }

            protected override void VisitComponent(IComponent component)
            {
                if (ComponentVisitor != null)
                    ComponentVisitor(component);

                base.VisitComponent(component);
            }

            protected override void VisitProperty(IProperty property)
            {
                if (PropertyVisitor != null)
                    PropertyVisitor(property);

                base.VisitProperty(property);
            }
        }
    }
}