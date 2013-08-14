namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;

    internal class Collection : Container, ICollection
    {
        public Collection(JObject collection)
            : base(collection)
        {
        }
    }
}