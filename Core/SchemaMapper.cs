namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class SchemaMapper
    {
        internal static void SynchProduct(Product product, IProductSchema schema)
        {
            product.Toolkit.Id = schema.Toolkit.Id;
            product.Toolkit.Version = schema.Toolkit.Version;

            SyncContainer(product, schema);
        }

        internal static void SyncCollection(Collection collection, ICollectionSchema schema)
        {
            // TODO: collection-specifc properties.
        }

        internal static void SyncElement(Element element, IElementSchema schema)
        {
        }

        private static void SyncContainer(Container container, IContainerSchema schema)
        {
            // Delete child elements that don't have their corresponding schema.
            container.Components
                .Where(c => !schema.Components.Any(i => i.Id == c.SchemaId))
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
            foreach (var component in container.Components.OfType<Component>())
            {
                SyncComponent(component, schema.Components.First(x => x.Id == component.SchemaId));
            }
        }

        private static void SyncComponent(Component component, IComponentSchema schema)
        {
            component.Schema = schema;

            // Delete existing properties that don't have a corresponding definition.
            component.Properties
                .Where(p => !schema.Properties.Any(info => info.Name == p.Name))
                .ToArray()
                .ForEach(p => p.Delete());

            // Initialize all the new properties. Existing ones are not modified.
            foreach (var info in schema.Properties.Where(info => !component.Properties.Any(p => p.Name == info.Name)))
            {
                // Assigning the DefinitionId on create automatically loads the Info property.
                var property = component.CreateProperty(info.Name);
                // Reset evaluates VP and default value.
                property.Reset();
            }
        }
    }
}