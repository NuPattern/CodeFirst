namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal abstract class Component : IComponent
    {
        private IComponentSchema schema;
        private ConcurrentDictionary<string, Property> properties = new ConcurrentDictionary<string, Property>();

        public Component(Component parent)
        {
            this.Parent = parent;
        }

        public IComponentSchema Schema
        {
            get { return schema; }
            set
            {
                schema = value;
                SchemaId = schema.Id;
            }
        }

        public string SchemaId { get; private set; }

        public string Name { get; set; }

        public IEnumerable<Property> Properties
        {
            get
            {
                return Enumerable.Empty<Property>();
            }
        }

        public Component Parent { get; private set; }

        public Product Product
        {
            get 
            {
                return this.Ancestors().OfType<Product>().FirstOrDefault();
            }
        }

        public Product Root
        {
            get
            {
                return this.Ancestors().OfType<Product>().LastOrDefault();
            }
        }

        public Property CreateProperty(string name)
        {
            // TODO: should check for duplicate property names
            // TODO: should retrieve schema for property
            // TODO: if no schema for property, consider it a dynamic property
            throw new NotImplementedException();
        }

        public void Delete()
        {
        }

        public virtual T Get<T>(string propertyName)
        {
            throw new NotImplementedException();
        }

        public virtual Component Set<T>(string propertyName, T value)
        {
            throw new NotImplementedException();
        }

        public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : InstanceVisitor;

        internal void DeleteProperty(Property property)
        {
            // A reverse index might speed up things, but removing properties 
            // is not a common operation, so this should be fine.
            var key = properties.Where(pair => pair.Value == property).Select(pair => pair.Key).FirstOrDefault();
            if (key != null)
                properties.TryRemove(key, out property);
        }

        private IPropertySchema FindSchema(string propertyName)
        {
            return schema == null ? null :
                schema.PropertySchemas.FirstOrDefault(p => p.PropertyName == propertyName);
        }

        IEnumerable<IProperty> IComponent.Properties { get { return Properties; } }
        IComponent IComponent.Parent { get { return Parent; } }
        IProduct IComponent.Product { get { return Product; } }
        IProduct IComponent.Root { get { return Root; } }

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