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

            var itemsProp = this.collection.Property("$Items");
            if (itemsProp == null)
            {
                this.items = new JArray();
                itemsProp = new JProperty("$Items", this.items);
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

            var element = new JObject(new JProperty("Name", name));
            this.items.Add(element);

            // TODO: can the item be a collection in turn?
            // TODO: set the item schema.
            return new Element(element) { SchemaId = this.Schema.ItemSchema };
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