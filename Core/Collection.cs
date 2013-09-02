namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Collection : Container, ICollection
    {
        public Collection(Component parent)
            : base(parent)
        {
        }

        public new ICollectionSchema Schema { get; set; }

        public IEnumerable<IElement> Items
        {
            get
            {
                throw new NotImplementedException();
                throw new NotImplementedException();
            }
        }

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitCollection(this);
            return visitor;
        }

        public IElement CreateItem(string name)
        {
            throw new NotImplementedException();

            //if (this.Schema.ItemSchema is ICollectionSchema)
            //    return SchemaMapper.SyncCollection(new Collection(json), (ICollectionSchema)this.Schema.ItemSchema);
            //else if (this.Schema.ItemSchema is IElementSchema)
            //return SchemaMapper.SyncElement(new Element(json), (IElementSchema)this.Schema.ItemSchema);

            // TODO: should never happen? BadSchemaException or the like?
            throw new NotSupportedException();
        }

        public new Collection Set<T>(string propertyName, T value)
        {
            base.Set(propertyName, value);
            return this;
        }

        IEnumerable<IElement> ICollection.Items { get { return Items; } }
        IElement ICollection.CreateItem(string name)
        {
            return CreateItem(name);
        }
        ICollection ICollection.Set<T>(string propertyName, T value)
        {
            return Set<T>(propertyName, value);
        }
    }
}