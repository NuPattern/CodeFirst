namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    internal class Property : IProperty
    {
        private JProperty property;

        public Property(JProperty property)
        {
            this.property = property;
        }

        public IComponent Owner
        {
            get { return ((JObject)property.Parent).AsComponent(); }
        }

        public object Value
        {
            get { return property.Values().OfType<JValue>().Select(v => v.Value).FirstOrDefault(); }
            set { property.Value = new JValue(value); }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public string Definition
        {
            get { return property.Name; }
        }
    }
}