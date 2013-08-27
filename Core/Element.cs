namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using NuPattern.Schema;
    using System;

    internal class Element : Container, IElement
    {
        public Element(JObject element)
            : this(element, null)
        {
        }

        public Element(JObject element, JProperty property)
            : base(element, property)
        {
        }

        public new IElementSchema Schema { get; set; }

        public new IElement Set<T>(string propertyName, T value)
        {
            base.Set(propertyName, value);
            return this;
        }
    }
}