namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    internal enum Kind
    {
        Value,
        Reference,
    }

    internal class Property : IProperty
    {
        private JProperty property;

        public Property(JProperty property)
        {
            this.property = property;
        }

        public string Name { get { return property.Name; } }

        public IComponent Owner
        {
            get { return ((JObject)property.Parent).AsComponent(); }
        }

        public object Value
        {
            get { return property.Values().OfType<JValue>().Select(v => v.Value).FirstOrDefault(); }
            set { property.Value = new JValue(value); }
        }

        public Kind Kind
        {
            get { return property.Value is JValue ? Kind.Value : Kind.Reference; }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}