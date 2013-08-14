namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    internal class JHiddenProperty : JProperty
    {
        public JHiddenProperty(string name, object content)
            : base(name, new JRaw(content))
        {
        }

        public override void WriteTo(Newtonsoft.Json.JsonWriter writer, params Newtonsoft.Json.JsonConverter[] converters)
        {
            // Never write it.
        }
    }
}