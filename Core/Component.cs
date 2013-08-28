namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal abstract class Component : IComponent
    {
        private JObject component;
        // The property where the component is used.
        // This allows renaming the property when 
        // the component name is changed.
        private JProperty property;

        private Func<string> getName;
        private Action<string> setName;
        private IComponentSchema schema;

        public Component(JObject component)
            : this(component, null)
        {
        }

        public Component(JObject component, JProperty property)
        {
            this.component = component;
            this.property = property;

            component.SetModel(this);

            if (property == null)
            {
                // We have a named item in a collection
                getName = () => component.Get(() => Name);
                setName = value => component.Set(() => Name, value);
            }
            else
            {
                // We have a property on a component
                getName = () => property.Name;
                setName = value => property.Replace(new JProperty(value, property.Value));
            }
        }

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { component.PropertyChanging += value; }
            remove { component.PropertyChanging -= value; }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { component.PropertyChanged += value; }
            remove { component.PropertyChanged -= value; }
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

        public string SchemaId
        {
            get { return component.Get<string>(Prop.Schema); }
            set { component.Set<string>(Prop.Schema, value); }
        }

        public string Name
        {
            get { return getName(); }
            set { setName(value); }
        }

        public IEnumerable<Property> Properties
        {
            get { return component.Properties().Where(x => x.Value is JValue).Select(x => new Property(x)); }
        }

        public Component Parent
        {
            get
            {
                return component.Parent == null ? null : component
                    .Ancestors().OfType<JObject>()
                    .Select(x => x.AsComponent()).FirstOrDefault();
            }
        }

        public Product Product
        {
            get
            {
                return component.Parent == null ? null : component
                    .Ancestors().OfType<JObject>()
                    .Select(x => x.AsComponent())
                    .OfType<Product>()
                    .FirstOrDefault();
            }
        }

        public Product Root
        {
            get
            {
                return component.Parent == null ? null : component
                    .Ancestors().OfType<JObject>()
                    .Select(x => x.AsComponent())
                    .OfType<Product>()
                    .LastOrDefault();
            }
        }

        public Property CreateProperty(string name)
        {
            // TODO: verify it doesn't exist already.
            var prop = new JProperty(name);
            component.Add(prop);
            return new Property(prop);
        }

        public void Delete()
        {
            component.Remove();
        }

        public virtual T Get<T>(string propertyName)
        {
            return component.Get<T>(propertyName);
        }

        public virtual Component Set<T>(string propertyName, T value)
        {
            component.Set(propertyName, value);
            return this;
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