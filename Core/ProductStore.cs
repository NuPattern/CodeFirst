namespace NuPattern
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class ProductStore : IProductStore
    {
        private string storeFile;
        private JArray json = new JArray();
        private List<IProduct> products = new List<IProduct>();
        private bool isOpen;

        public ProductStore(string storeFile, IEnumerable<IToolkitSchema> toolkits)
        {
            Guard.NotNullOrEmpty(() => storeFile, storeFile);
            Guard.NotNull(() => toolkits, toolkits);

            this.storeFile = storeFile;
            this.Toolkits = toolkits;
        }

        public IEnumerable<IProduct> Products 
        {
            get 
            {
                EnsureOpen();
                return this.products; 
            } 
        }

        public IEnumerable<IToolkitSchema> Toolkits { get; private set; }

        public IProduct CreateProduct(string name, string schemaId)
        {
            Guard.NotNullOrEmpty(() => name, name);
            Guard.NotNullOrEmpty(() => schemaId, schemaId);

            var schema = Toolkits.SelectMany(x => x.ProductSchemas).FirstOrDefault(x => x.Id == schemaId);
            if (schema == null)
                throw new ArgumentException();

            EnsureOpen();

            var jprod = new JObject();
            json.Add(jprod);
            var prod = new Product(new JObject()) { Name = name };

            SchemaMapper.SynchProduct(prod, schema);

            return prod;
        }

        public void Save()
        {
            EnsureOpen();
            var contents = JsonConvert.SerializeObject(json);
            File.WriteAllText(storeFile, contents);
        }

        private void EnsureOpen()
        {
            if (!isOpen)
            {
                if (File.Exists(storeFile))
                {
                    // TODO: use more efficient than large string.
                    var contents = File.ReadAllText(storeFile);
                    json = (JArray)JsonConvert.DeserializeObject(contents) ?? new JArray();
                    // TODO: must reload all IProducts, set their schemas, initialize the 
                    // properties, etc. etc.
                    // Maybe we do all that lazily?
                }
                else
                {
                    json = new JArray();
                }

                isOpen = true;
            }
        }
    }
}