namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;

    internal class Element : Container, IElement
    {
        public Element(JObject element)
            : base(element)
        {
        }
    }
}