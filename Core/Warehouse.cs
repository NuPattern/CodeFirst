namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Warehouse : IWarehouse
    {
        private List<IProductStore> stores = new List<IProductStore>();
        private IProductSerializer serializer;

        public event ValueEventHandler<IProductStore> Closed = (sender, args) => { };
        public event ValueEventHandler<IProductStore> Opening = (sender, args) => { };
        public event ValueEventHandler<IProductStore> Opened = (sender, args) => { };
        public event ValueEventHandler<IProductStore> Saved = (sender, args) => { };

        public Warehouse(IProductSerializer serializer)
        {
            Guard.NotNull(() => serializer, serializer);

            this.serializer = serializer;
        }

        public void Open(string storeFile)
        {
        }

        public void SaveAll()
        {
            // TODO: report saving progress.
            stores.ToArray().ForEach(store =>
                {
                    store.Save(Progress.Of<int>(i => { }));
                    Saved(this, ValueEventArgs.Create(store));
                });
        }

        public void CloseAll()
        {
            stores.ToArray().ForEach(store => store.Close());
        }

        public IEnumerable<IProductStore> Stores
        {
            get { throw new NotImplementedException(); }
        }
    }
}
