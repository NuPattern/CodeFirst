namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;

    public interface ICollection : IContainer
    {
        event ValueEventHandler<IElement> ItemAdded;
        event ValueEventHandler<IElement> ItemRemoved;

        IEnumerable<IElement> Items { get; }
        new ICollectionInfo Schema { get; }

        // The definition of the items is taken from 
        // the collection schema ItemId property.
        IElement CreateItem(string name, string schemaId);

        new ICollection Set<T>(string propertyName, T value);
    }
}