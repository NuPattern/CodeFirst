namespace NuPattern
{
    using NuPattern.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal abstract class Container : Component, IContainer
    {
        private List<Component> components = new List<Component>();

        public Container(string name, string schemaId, Component parent)
            : base(name, schemaId, parent)
        {
        }

        public IEnumerable<Component> Components
        {
            get { return this.components; }
        }

        public new IContainerSchema Schema
        {
            get { return (IContainerSchema)base.Schema; }
            set { base.Schema = value; }
        }

        public Collection CreateCollection(string name, string schemaId)
        {
            ThrowIfDuplicate(name);
            var collection = new Collection(name, schemaId, this);

            if (Schema != null)
            {
                var schema = Schema.ComponentSchemas
                    .OfType<ICollectionSchema>()
                    .FirstOrDefault(x => x.SchemaId == schemaId);
                if (schema != null)
                    SchemaMapper.SyncCollection(collection, schema);
            }

            components.Add(collection);
            return collection;
        }

        public Element CreateElement(string name, string schemaId)
        {
            ThrowIfDuplicate(name);
            var element = new Element(name, schemaId, this);

            if (Schema != null)
            {
                var schema = Schema.ComponentSchemas
                    .OfType<IElementSchema>()
                    .FirstOrDefault(x => x.SchemaId == schemaId);
                if (schema != null)
                    SchemaMapper.SyncElement(element, schema);
            }

            components.Add(element);
            return element;
        }

        internal void DeleteComponent(Component component)
        {
            components.Remove(component);
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                foreach (var component in components.ToArray())
                {
                    component.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void ThrowIfDuplicate(string name)
        {
            if (components.Any(c => c.Name == name))
                throw new ArgumentException(Strings.Container.DuplicateComponentName(name));
            if (Properties.Any(p => p.Name == name))
                throw new ArgumentException(Strings.Container.ComponentNameMatchesProperty(name, Name));
        }

        internal void ThrowIfDuplicateRename(string oldName, string newName)
        {
            if (components.Any(c => c.Name == newName))
                throw new ArgumentException(Strings.Container.RenamedDuplicateComponent(oldName, newName, Name));
            if (Properties.Any(p => p.Name == newName))
                throw new ArgumentException(Strings.Container.RenamedDuplicateProperty(oldName, newName, Name));
        }

        IEnumerable<IComponent> IContainer.Components { get { return Components; } }
        ICollection IContainer.CreateCollection(string name, string definition)
        {
            return CreateCollection(name, definition);
        }
        IElement IContainer.CreateElement(string name, string definition)
        {
            return CreateElement(name, definition);
        }
    }
}