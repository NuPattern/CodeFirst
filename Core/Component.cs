namespace NuPattern
{
    using NuPattern.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal abstract class Component : IComponent, IDisposable
    {
        private Dictionary<string, Property> properties = new Dictionary<string, Property>();
        private string name;

        public event EventHandler Disposed = (sender, args) => { };

        public Component(string name, string schemaId, Component parent)
        {
            this.Name = name;
            this.SchemaId = schemaId;
            this.Parent = parent;
        }

        public bool IsDisposed { get; private set; }

        public IComponentSchema Schema { get; internal set; }

        public string SchemaId { get; private set; }

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    OnRenaming(name, value);
                    name = value;
                }
            }
        }

        public IEnumerable<Property> Properties
        {
            get { return properties.Values; }
        }

        public Component Parent { get; private set; }

        public Product Product
        {
            get
            {
                return this.Ancestors().OfType<Product>().FirstOrDefault();
            }
        }

        //public Product Root
        //{
        //    get
        //    {
        //        return this.Ancestors().OfType<Product>().LastOrDefault();
        //    }
        //}

        public virtual Property CreateProperty(string name)
        {
            if (name == "Name")
                throw new ArgumentException(Strings.Component.NamePropertyReserved);
            if (properties.ContainsKey(name))
                throw new ArgumentException(Strings.Component.DuplicatePropertyName(name));

            var property = new Property(name, this);
            properties[name] = property;
            if (this.Schema != null)
                property.Schema = this.Schema.PropertySchemas.FirstOrDefault(x => x.Name == name);
            // TODO: if no schema for property, consider it a dynamic property?
            // Should we always have a schema? (null object pattern?)

            return property;
        }

        public void Delete()
        {
            Dispose();
        }

        public T Get<T>(string propertyName)
        {
            Property property;
            if (properties.TryGetValue(propertyName, out property))
                return property.Value == null ? default(T) : (T)property.Value;

            return default(T);
        }

        public Component Set<T>(string propertyName, T value)
        {
            properties.GetOrAdd(propertyName, name => CreateProperty(name)).Value = value;
            return this;
        }

        public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : InstanceVisitor;

        public override string ToString()
        {
            return Name + " : " + SchemaId;
        }

        public void Dispose()
        {
            Dispose(true);
            Disposed(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose automation, unsubscribe events, etc.
                }

                IsDisposed = true;
            }

            var container = this.Parent as Container;
            if (container != null)
                container.DeleteComponent(this);

            this.Parent = null;
        }

        protected virtual void OnRenaming(string oldName, string newName)
        {
            var container = Parent as Container;
            if (container != null)
                container.ThrowIfDuplicateRename(oldName, newName);
        }

        internal void DeleteProperty(Property property)
        {
            properties.Remove(property.Name);
        }

        IEnumerable<IProperty> IComponent.Properties { get { return Properties; } }
        IComponent IComponent.Parent { get { return Parent; } }
        IProduct IComponent.Product { get { return Product; } }
        //IProduct IComponent.Root { get { return Root; } }

        IProperty IComponent.CreateProperty(string name)
        {
            return CreateProperty(name);
        }

        IComponent IComponent.Set<T>(string propertyName, T value)
        {
            return Set<T>(propertyName, value);
        }
    }
}