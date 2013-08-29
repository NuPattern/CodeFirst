namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal abstract class Component : IComponent, ISupportInitialize, ISupportInitializeNotification
    {
        private JObject component;
        // The property where the component is used.
        // This allows renaming the property when 
        // the component name is changed.
        private JProperty property;

        private Func<string> getName;
        private Action<string> setName;
        private IComponentSchema schema;
        private ConcurrentDictionary<JProperty, Property> properties = new ConcurrentDictionary<JProperty, Property>();

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
                getName = () => property.Name;
                setName = value =>
                {
                    var newProp = new JProperty(value, property.Value);
                    // TODO: JLinq already verifies duplicate properties
                    // but we should throw with a NuPattern-specific message.
                    // We have a property on a component
                    property.Replace(newProp);
                    property = newProp;
                };
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
            get
            {
                return component.Properties()
                    .Where(x => !x.Name.StartsWith("$") && x.Value is JValue)
                    .Select(x => properties.GetOrAdd(x, j => new Property(j)));
            }
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
            // TODO: JLinq already checks for duplicate props, 
            // but we should throw a NuPattern-specific exception.
            var prop = new JProperty(name, "");
            component.Add(prop);

            // If component has already been initialized, we 
            // sort now, otherwise, we defer to when initialization 
            // is finished.
            if (IsInitialized)
                SortProperties();

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
                schema.Properties.FirstOrDefault(p => p.Name == propertyName);
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

        public event EventHandler Initialized = (sender, args) => { };

        public virtual void BeginInit()
        {
            IsInitialized = false;
        }

        public virtual void EndInit()
        {
            SortProperties();
            IsInitialized = true;
            Initialized(this, EventArgs.Empty);
        }

        private void SortProperties()
        {
            // Reorder properties. This is costly, but it's done 
            // by the schema sync only the first time 
            // a certain property is created.
            var jprops = component.Properties().OrderBy(p => p.Name).ToList();
            jprops.ForEach(p => p.Remove());
            jprops.ForEach(p => component.Add(p));
        }

        public bool IsInitialized { get; private set; }
    }
}