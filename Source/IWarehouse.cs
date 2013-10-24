using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuPattern
{
    // ISolutionViewModel
    // event ElementActivated;
    // event CollectionChanged (index, added/removed, etc.)

    public interface IWarehouse
    {
        // event ElementCreated; -> Traverse(tree) -> nodo.Remove();
        // event ElementDeleted;
        // event ElementDeleting;
        // event ElementInstantiated;
        // event ElementLoaded;
        // event ElementPropertyChanged;
                // Collection.Items prop?
                // Container.Elements ?
        // event CollectionChanged ?

        // ctor(IElement) => propertychanged (port) -> IIS configure
        // IElement.PropertyChanged. 
        // IWebService
        //          PropertyChanged("Storage")
        //          Changed("Items") -> Changed("Buckets")
        //      IStorage Storage
        //          PropertyChanged("Name")                    
        //          Name { get; set; }
        //      IBuckets Buckets       -> Changed("Items")
        //          IBucket[0].Port     -> Changed("Port")

        // Open(storeFile);
        // SaveAll();
        // CloseAll();


        IEnumerable<IProductStore> Stores { get; }
    }
}