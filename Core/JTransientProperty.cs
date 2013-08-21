namespace NuPattern
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    internal class JTransientProperty : JProperty
    {
        public JTransientProperty(string name, object content)
            : base(name, new JRaw(content))
        {
        }

        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            // Never write it.
        }
    }
}