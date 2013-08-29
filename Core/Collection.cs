namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Collection : Container, ICollection
    {
        private JObject collection;
        private JArray items;

        public Collection(JObject collection)
            : this(collection, null)
        {
        }

        public Collection(JObject collection, JProperty property)
            : base(collection, property)
        {
            this.collection = collection;

            var itemsProp = this.collection.Property(Prop.Items);
            if (itemsProp == null)
            {
                this.items = new JArray();
                itemsProp = new JProperty(Prop.Items, this.items);
                this.collection.Add(itemsProp);
            }
            else
            {
                this.items = (JArray)itemsProp.Value;
            }
        }

        public new ICollectionSchema Schema { get; set; }

        public IEnumerable<Component> Items
        {
            get
            {
                return items.Children<JObject>().Select(x => x.AsComponent());
            }
        }

        public Component CreateItem(string name)
        {
            if (this.Schema == null)
                throw new InvalidOperationException();
            if (this.Schema.Items == null)
                throw new InvalidOperationException();

            // TODO: check for duplicate names.

            var json = new JObject(new JProperty("Name", name));
            this.items.Add(json);

            if (this.Schema.Items is ICollectionSchema)
                return SchemaMapper.SyncCollection(new Collection(json), (ICollectionSchema)this.Schema.Items);
            else if (this.Schema.Items is IElementSchema)
                return SchemaMapper.SyncElement(new Element(json), (IElementSchema)this.Schema.Items);

            // TODO: should never happen? BadSchemaException or the like?
            throw new NotSupportedException();
        }

        public new Collection Set<T>(string propertyName, T value)
        {
            base.Set(propertyName, value);
            return this;
        }

        IEnumerable<IComponent> ICollection.Items { get { return Items; } }
        IComponent ICollection.CreateItem(string name)
        {
            return CreateItem(name);
        }

        ICollection ICollection.Set<T>(string propertyName, T value)
        {
            return Set<T>(propertyName, value);
        }
    }
}