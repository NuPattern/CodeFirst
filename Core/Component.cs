namespace NuPattern
{
    using NuPattern.Core.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal abstract class Component : IComponent, IDisposable, ILineInfo
    {
        private Dictionary<string, Property> properties = new Dictionary<string, Property>();
        private List<IAutomation> automations = new List<IAutomation>();
        private object annotations;
        private string name;

        public event EventHandler Deleted = (sender, args) => { };

        public event EventHandler Disposed = (sender, args) => { };

        public event EventHandler<PropertyChangeEventArgs> PropertyChanged = (sender, args) => { };

        public event EventHandler<PropertyChangeEventArgs> PropertyChanging = (sender, args) => { };

        public Component(string name, string schemaId, Component parent)
        {
            this.Name = name;
            this.SchemaId = schemaId;
            this.Parent = parent;
        }

        public bool IsDisposed { get; private set; }

        public IComponentContext Context { get; internal set; }

        public IComponentInfo Schema { get; internal set; }

        public string SchemaId { get; private set; }

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    var oldValue = name;
                    OnRenaming(oldValue, value);
                    RaisePropertyChanging("Name", oldValue, value);
                    name = value;
                    RaisePropertyChanged("Name", oldValue, value);
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

        public IEnumerable<IAutomation> Automations { get { return automations; } }

        public void AddAutomation(IAutomation automation)
        {
            Guard.NotNull(() => automation, automation);

            automations.Add(automation);
        }

        public T As<T>() where T : class
        {
            return (T)new SmartCast().Cast(this, typeof(T));
        }

        public virtual Property CreateProperty(string name)
        {
            if (name == "Name")
                throw new ArgumentException(Strings.Component.NamePropertyReserved);
            if (properties.ContainsKey(name))
                throw new ArgumentException(Strings.Component.DuplicatePropertyName(name));

            var property = new Property(name, this);
            properties[name] = property;
            if (Schema != null)
                property.Schema = Schema.Properties.FirstOrDefault(x => x.Name == name);

            // TODO: if no schema for property, consider it a dynamic property?
            // Should we always have a schema? (null object pattern?)

            return property;
        }

        public void Delete()
        {
            var container = Parent as Container;
            var collection = Parent as Collection;
            if (collection != null)
                collection.DeleteItem(this);
            else if (container != null)
                container.DeleteComponent(this);

            Deleted(this, EventArgs.Empty);
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

        #region ILineInfo

        public bool HasLineInfo { get { return LinePosition.HasValue && LineNumber.HasValue; }  }

        public int? LinePosition { get; private set; }

        public int? LineNumber { get; private set; }

        internal void SetLineInfo(int lineNumber, int linePosition)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        #endregion  

        #region Annotations

        public void AddAnnotation(object annotation)
        {
            Annotator.AddAnnotation(ref annotations, annotation);
        }

        public object Annotation(Type type)
        {
            return Annotator.Annotation(annotations, type);
        }

        public IEnumerable<object> Annotations(Type type)
        {
            return Annotator.Annotations(annotations, type);
        }

        public void RemoveAnnotations(Type type)
        {
            Annotator.RemoveAnnotations(ref annotations, type);
        }

        #endregion

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
                    foreach (var automation in automations.OfType<IDisposable>())
                    {
                        automation.Dispose();
                    }
                }

                IsDisposed = true;
            }

            Parent = null;
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

        internal void RaisePropertyChanging(string propertyName, object oldValue, object newValue)
        {
            PropertyChanging(this, new PropertyChangeEventArgs(propertyName, oldValue, newValue));
        }

        internal void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            PropertyChanged(this, new PropertyChangeEventArgs(propertyName, oldValue, newValue));
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