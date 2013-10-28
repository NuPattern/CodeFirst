namespace NuPattern
{
    using NuPattern.Core.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Container : Component, IContainer
    {
        private List<Component> components = new List<Component>();

        public event ValueEventHandler<IComponent> ComponentAdded = (sender, args) => { };
        public event ValueEventHandler<IComponent> ComponentRemoved = (sender, args) => { };

        public Container(string name, string schemaId, Component parent)
            : base(name, schemaId, parent)
        {
        }

        public IEnumerable<Component> Components
        {
            get { return components; }
        }

        public new IContainerInfo Schema
        {
            get { return (IContainerInfo)base.Schema; }
            internal set { base.Schema = value; }
        }

        public Collection CreateCollection(string name, string schemaId)
        {
            ThrowIfDuplicate(name);
            var collection = new Collection(name, schemaId, this);

            if (Schema != null)
            {
                var schema = Schema.Components
                    .OfType<ICollectionInfo>()
                    .FirstOrDefault(x => x.SchemaId == schemaId);
                if (schema != null)
                    ComponentMapper.SyncCollection(collection, schema);
            }

            collection.Events.PropertyChanged += OnComponentChanged;
            collection.Events.Deleted += OnComponentDeleted;
            components.Add(collection);
            ComponentAdded(this, collection);

            return collection;
        }

        public Element CreateElement(string name, string schemaId)
        {
            ThrowIfDuplicate(name);
            var element = new Element(name, schemaId, this);

            if (Schema != null)
            {
                var schema = Schema.Components
                    .OfType<IElementInfo>()
                    .FirstOrDefault(x => x.SchemaId == schemaId);
                if (schema != null)
                    ComponentMapper.SyncElement(element, schema);
            }

            element.Events.PropertyChanged += OnComponentChanged;
            element.Events.Deleted += OnComponentDeleted;
            components.Add(element);
            ComponentAdded(this, element);

            return element;
        }

        internal void DeleteComponent(Component component)
        {
            // After the delete, the component is disposed, which will 
            // call our OnComponentDisposed, at which point we unsubscribe 
            // the property changed event.
            components.Remove(component);
            ComponentRemoved(this, component);
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

        private void OnComponentChanged(object sender, EventArgs args)
        {
            var component = (IComponent)sender;
            Events.RaisePropertyChanged(component.Name, sender, sender);
        }

        private void OnComponentDeleted(object sender, EventArgs args)
        {
            ((IComponent)sender).Events.PropertyChanged -= OnComponentChanged;
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