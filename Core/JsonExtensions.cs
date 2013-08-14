namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    internal static class JsonExtensions
    {
        public static Component AsComponent(this JObject component)
        {
            var result = (Component)component.GetModel();
            if (result == null)
            {
                var definition = (string)component.Property("Definition").Value;
                if (definition == "Element")
                    result = new Element(component);
                else if (definition == "Collection")
                    result = new Collection(component);
            }

            return result;
        }

        public static Product AsProduct(this JObject component)
        {
            return (Product)component.GetModel() ?? new Product(component);
        }

        public static object GetModel(this JObject json)
        {
            var property = json.Property("$model");
            if (property == null)
                return null;

            return ((JValue)property.Value).Value;
        }

        public static void SetModel(this JObject json, object model)
        {
            var property = json.Property("$model");
            if (property == null)
                json.Add(new JHiddenProperty("$model", model));
            else
                property.Value = new JRaw(model);
        }
    }


}