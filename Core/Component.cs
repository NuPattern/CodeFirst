namespace NuPattern
{
    using NuPattern.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal abstract class Component : IComponent
    {
        private ConcurrentDictionary<string, Property> properties = new ConcurrentDictionary<string, Property>();

        public Component(string name, string schemaId, Component parent)
        {
            this.Name = name;
            this.SchemaId = schemaId;
            this.Parent = parent;
        }

        public IComponentSchema Schema { get; internal set; }

        public string SchemaId { get; private set; }

        public string Name { get; set; }

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

        public Property CreateProperty(string name)
        {
            if (properties.ContainsKey(name))
                throw new ArgumentException(Strings.Component.DuplicatePropertyName(name));

            // TODO: should retrieve schema for property
            // TODO: if no schema for property, consider it a dynamic property

            var property = new Property(name, this);
            properties[name] = property;
            if (this.Schema != null)
                property.Schema = this.Schema.PropertySchemas.FirstOrDefault(x => x.PropertyName == name);

            return property;
        }

        public void Delete()
        {
            var container = this.Parent as Container;
            if (container != null)
                container.DeleteComponent(this);

            this.Parent = null;
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

        internal void DeleteProperty(Property property)
        {
            properties.TryRemove(property.Name, out property);
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