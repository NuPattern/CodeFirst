using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuPattern
{
    public interface IWarehouse
    {
        event ValueEventHandler<IProductStore> Closed;
        event ValueEventHandler<IProductStore> Opening;
        event ValueEventHandler<IProductStore> Opened;
        event ValueEventHandler<IProductStore> Saved;

        // event ElementCreated; -> Traverse(tree) -> nodo.Remove();
        // event ElementDeleted;
        // event ElementDeleting;
        // event ElementInstantiated;
        // event ElementLoaded;
        // event ElementPropertyChanged;
        // Collection.Items prop?
        // Container.Elements ?
        // event CollectionChanged ?

        void Open(string storeFile);

        void SaveAll();
        void CloseAll();

        IEnumerable<IProductStore> Stores { get; }
    }
}