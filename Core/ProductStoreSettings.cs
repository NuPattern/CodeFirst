namespace NuPattern
{
    using System;
    using System.Linq;

    internal class ProductStoreSettings
    {
        public ProductStoreSettings(string storeName, string stateFile)
        {
            Guard.NotNullOrEmpty(() => storeName, storeName);
            Guard.NotNullOrEmpty(() => stateFile, stateFile);

            this.StoreName = storeName;
            this.StateFile = stateFile;            
        }

        public string StoreName { get; private set; }
        public string StateFile { get; private set; }
    }
}