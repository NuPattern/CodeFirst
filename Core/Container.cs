namespace NuPattern
{
    using NuPattern.Properties;
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
            get { return this.components.AsReadOnly(); }
        }

        public Collection CreateCollection(string name, string schemaId)
        {
            ThrowIfDuplicate(name);
            var collection = new Collection(name, schemaId, this);
            components.Add(collection);
            return collection;
        }

        public Element CreateElement(string name, string schemaId)
        {
            ThrowIfDuplicate(name);
            var element = new Element(name, schemaId, this);
            components.Add(element);
            return element;
        }

        internal void DeleteComponent(Component component)
        {
            components.Remove(component);
        }

        private void ThrowIfDuplicate(string name)
        {
            if (components.Any(c => c.Name == name))
                throw new ArgumentException(Strings.Container.DuplicateComponentName(name));
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