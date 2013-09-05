namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal interface IProductSerializer
    {
        void Serialize(TextWriter writer, IEnumerable<Product> products);
        IEnumerable<Product> Deserialize(TextReader reader);
    }
}