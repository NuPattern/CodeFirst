namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class ComponentMapper
    {
        public static Product SyncProduct(Product product, IProductInfo schema)
        {
            product.Toolkit.Id = schema.Toolkit.Id;
            product.Toolkit.Version = schema.Toolkit.Version;

            SyncContainer(product, schema);

            return product;
        }

        public static Collection SyncCollection(Collection collection, ICollectionInfo schema)
        {
            SyncContainer(collection, schema);
            foreach (var item in collection.Items)
            {
                SyncElement(item, schema.Item);
            }

            return collection;
        }

        public static Element SyncElement(Element element, IElementInfo schema)
        {
            SyncContainer(element, schema);

            return element;
        }

        private static void SyncContainer(Container container, IContainerInfo schema)
        {
            // Delete child elements that have a schema id but a matching 
            // one does not exist anymore.
            container.Components
                .OfType<ICollection>()
                .Where(c => !schema.Components.OfType<ICollectionInfo>().Any(i => i.SchemaId == c.SchemaId))
                .ToArray()
                .ForEach(c => c.Delete());
            container.Components
                .OfType<IElement>()
                .Where(c => !schema.Components.OfType<IElementInfo>().Any(i => i.SchemaId == c.SchemaId))
                .ToArray()
                .ForEach(c => c.Delete());

            // TODO: Delete extra child elements that are beyond the specified cardinality.
            //container.Components
            //    .GroupBy(x => x.SchemaId)
            //    .Where(x => schema.Components.Any(
            //        i => x.Key == i.Id &&
            //        (i.Cardinality == Cardinality.OneToOne || i.Cardinality == Cardinality.ZeroToOne) &&
            //        x.Count() > 1))
            //    .SelectMany(x => x.Skip(1))
            //    .ForEach(x => x.Delete());

            // TODO: auto-create elements where specified.
            //var singletonElements = schema.Components
            //    .Where(i => i.AutoCreate &&
            //        !container.Components.Any(e => e.SchemaId == i.Id))
            //    .ToArray();

            // TODO: also sync the newly created elements?
            //singletonElements.OfType<IElementInfo>().ForEach(x => element.CreateElement(e => e.DefinitionId = x.Id));
            //singletonElements.OfType<ICollectionInfo>().ForEach(x => element.CreateCollection(e => e.DefinitionId = x.Id));

            SyncComponent(container, schema);
            foreach (var component in container.Components)
            {
                var collection = component as Collection;
                var element = component as Element;

                if (collection != null)
                    SyncCollection(collection, schema.Components.OfType<ICollectionInfo>().First(x => x.SchemaId == component.SchemaId));
                else if (element != null)
                    SyncElement(element, schema.Components.OfType<IElementInfo>().First(x => x.SchemaId == component.SchemaId));
            }
        }

        private static void SyncComponent(Component component, IComponentInfo schema)
        {
            component.Schema = schema;

            // Delete existing properties that don't have a corresponding definition.
            // and are not system properties (starting with $) or hidden ones (starting with _)
            component.Properties
                .Where(p => !p.Name.StartsWith("$") && !p.Name.StartsWith("_") && !schema.Properties.Any(info => info.Name == p.Name))
                .ToArray()
                .ForEach(p => p.Delete());

            // Initialize all the new properties. Existing ones are not modified.
            foreach (var propertySchema in schema.Properties)
            {
                var property = component.Properties.FirstOrDefault(x => x.Name == propertySchema.Name);
                if (property != null)
                {
                    property.Schema = propertySchema;
                }
                else
                {
                    property = component.CreateProperty(propertySchema.Name);
                    property.Schema = propertySchema;
                }

                // NOTE: unlike original NuPattern, we don't eagerly evaluate default values, 
                // neither do we persist them.
            }
        }
    }
}