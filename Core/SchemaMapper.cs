namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class SchemaMapper
    {
        public static Product SyncProduct(Product product, IProductSchema schema)
        {
            product.Toolkit.Id = schema.ToolkitSchema.Id;
            product.Toolkit.Version = schema.ToolkitSchema.Version;

            SyncContainer(product, schema);

            return product;
        }

        public static Collection SyncCollection(Collection collection, ICollectionSchema schema)
        {
            SyncContainer(collection, schema);
            foreach (var item in collection.Items)
            {
                SyncElement(item, schema.ItemSchema);
            }

            return collection;
        }

        public static Element SyncElement(Element element, IElementSchema schema)
        {
            SyncContainer(element, schema);

            return element;
        }

        private static void SyncContainer(Container container, IContainerSchema schema)
        {
            // Delete child elements that have a schema id but a matching 
            // one does not exist anymore.
            container.Components
                .OfType<ICollection>()
                .Where(c => !schema.ComponentSchemas.OfType<ICollectionSchema>().Any(i => i.Id == c.SchemaId))
                .ToArray()
                .ForEach(c => c.Delete());
            container.Components
                .OfType<IElement>()
                .Where(c => !schema.ComponentSchemas.OfType<IElementSchema>().Any(i => i.Id == c.SchemaId))
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
                    SyncCollection(collection, schema.ComponentSchemas.OfType<ICollectionSchema>().First(x => x.Id == component.SchemaId));
                else if (element != null)
                    SyncElement(element, schema.ComponentSchemas.OfType<IElementSchema>().First(x => x.Id == component.SchemaId));
            }
        }

        private static void SyncComponent(Component component, IComponentSchema schema)
        {
            component.Schema = schema;

            // Delete existing properties that don't have a corresponding definition.
            // and are not system properties (starting with $)
            component.Properties
                .Where(p => !p.Name.StartsWith("$") && !schema.PropertySchemas.Any(info => info.PropertyName == p.Name))
                .ToArray()
                .ForEach(p => p.Delete());

            // Initialize all the new properties. Existing ones are not modified.
            foreach (var propertySchema in schema.PropertySchemas)
            {
                var property = component.Properties.FirstOrDefault(x => x.Name == propertySchema.PropertyName);
                if (property != null)
                {
                    property.Schema = propertySchema;
                }
                else
                {
                    property = component.CreateProperty(propertySchema.PropertyName);
                    property.Schema = propertySchema;
                }

                // NOTE: unlike original NuPattern, we don't eagerly evaluate default values, 
                // neither do we persist them.
            }
        }
    }
}