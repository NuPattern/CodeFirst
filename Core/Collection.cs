namespace NuPattern
{
    using NuPattern.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Collection : Container, ICollection
    {
        private List<Element> items = new List<Element>();

        public Collection(string name, string schemaId, Component parent)
            : base(name, schemaId, parent)
        {
            Guard.NotNull(() => parent, parent);
        }

        public new ICollectionSchema Schema
        {
            get { return (ICollectionSchema)base.Schema; }
            set { base.Schema = value; }
        }

        public IEnumerable<Element> Items
        {
            get { return items; }
        }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitCollection(this);
            return visitor;
        }

        public Element CreateItem(string name, string schemaId)
        {
            if (items.Any(x => x.Name == name))
                throw new ArgumentException(Strings.Collection.DuplicateItemName(name));

            var element = new Element(name, schemaId, this);

            if (Schema != null && Schema.ItemSchema != null)
                ComponentMapper.SyncElement(element, Schema.ItemSchema);

            element.PropertyChanged += OnItemChanged;
            element.Disposed += OnItemDisposed;
            items.Add(element);
            return element;
        }

        public new Collection Set<T>(string propertyName, T value)
        {
            base.Set(propertyName, value);
            return this;
        }

        public override string ToString()
        {
            return base.ToString() + " [" + Items.Count() + "]";
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                foreach (var item in items)
                {
                    item.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        internal void DeleteItem(Component component)
        {
            // After the delete, the component is disposed, which will 
            // call our OnComponentDisposed, at which point we unsubscribe 
            // the property changed event.
            items.Remove((Element)component);
        }

        private void OnItemChanged(object sender, EventArgs args)
        {
            RaisePropertyChanged("Items", sender, sender);
        }

        private void OnItemDisposed(object sender, EventArgs args)
        {
            ((Component)sender).PropertyChanged -= OnItemChanged;
        }

        IEnumerable<IElement> ICollection.Items { get { return Items; } }
        IElement ICollection.CreateItem(string name, string schemaId)
        {
            return CreateItem(name, schemaId);
        }
        ICollection ICollection.Set<T>(string propertyName, T value)
        {
            return Set<T>(propertyName, value);
        }
    }
}