namespace NuPattern
{
    using NuPattern.Core.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Collection : Container, ICollection
    {
        private List<Element> items = new List<Element>();

        public event ValueEventHandler<IElement> ItemAdded = (sender, args) => { };
        public event ValueEventHandler<IElement> ItemRemoved = (sender, args) => { };

        public Collection(string name, string schemaId, Component parent)
            : base(name, schemaId, parent)
        {
            Guard.NotNull(() => parent, parent);
        }

        public new ICollectionInfo Schema
        {
            get { return (ICollectionInfo)base.Schema; }
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

            if (Schema != null && Schema.Item != null)
                ComponentMapper.SyncElement(element, Schema.Item);

            element.Events.PropertyChanged += OnItemChanged;
            element.Events.Deleted += OnItemDeleted;
            items.Add(element);

            ItemAdded(this, element);

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
            var element = (Element)component;

            items.Remove(element);
            ItemRemoved(this, element);
        }

        private void OnItemChanged(object sender, EventArgs args)
        {
            Events.RaisePropertyChanged("Items", sender, sender);
        }

        private void OnItemDeleted(object sender, EventArgs args)
        {
            ((IComponent)sender).Events.PropertyChanged -= OnItemChanged;
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