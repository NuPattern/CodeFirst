namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal abstract class Container : Component, IContainer
    {
        private JObject container;

        public Container(JObject container)
            : base(container)
        {
            this.container = container;
        }

        public IEnumerable<IComponent> Components
        {
            get { return container.GetValue("Components").OfType<JObject>().Select(x => x.AsComponent()); }
        }

        public IEnumerable<IProduct> Extensions
        {
            get { return container.GetValue("Extensions").OfType<JObject>().Select(x => x.AsProduct()); }
        }

        public ICollection CreateCollection(string name, string definition)
        {
            return new Collection(AddObject(name, definition));
        }

        public IElement CreateElement(string name, string definition)
        {
            return new Element(AddObject(name, definition));
        }

        public IProduct CreateExtension(string name, string definition)
        {
            return new Product(AddObject(name, definition));
        }

        private JObject AddObject(string name, string definition)
        {
            var json = new JObject(
                new JProperty("Name", name),
                new JProperty("Definition", definition));

            container.Add(json);

            return json;
        }
    }
}