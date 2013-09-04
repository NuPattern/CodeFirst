namespace NuPattern.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IProductSerializer
    {
        void Serialize(TextWriter writer, IEnumerable<IProduct> products);
        IEnumerable<IProduct> Deserialize(TextReader reader);
    }
}