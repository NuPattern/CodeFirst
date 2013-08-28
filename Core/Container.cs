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
            : this(container, null)
        {
        }

        public Container(JObject container, JProperty property)
            : base(container, property)
        {
            this.container = container;
        }

        IEnumerable<IComponent> IContainer.Components { get { return Components; } }
        IEnumerable<IProduct> IContainer.Extensions { get { return Extensions; } }

        ICollection IContainer.CreateCollection(string name, string definition)
        {
            return CreateCollection(name, definition);
        }

        ICollection IContainer.CreateCollection(string name)
        {
            return CreateCollection(name);
        }

        IElement IContainer.CreateElement(string name, string definition)
        {
            return CreateElement(name, definition);
        }

        IProduct IContainer.CreateExtension(string name, string definition)
        {
            return CreateExtension(name, definition);
        }

        public IEnumerable<Component> Components
        {
            get
            {
                return container.Children<JProperty>()
                    .Where(x =>
                        !x.Name.StartsWith("$") &&
                        x.Value is JObject &&
                        ((JObject)x.Value).Property(Prop.Toolkit) == null)
                    .Select(x => ((JObject)x.Value).AsComponent(x));
            }
        }

        public IEnumerable<Product> Extensions
        {
            get
            {
                return container.Children<JProperty>()
                    .Where(x =>
                        !x.Name.StartsWith("$") &&
                        x.Value is JObject &&
                        ((JObject)x.Value).Property(Prop.Toolkit) != null)
                    .Select(x => ((JObject)x.Value).AsExtension(x));
            }
        }

        public Collection CreateCollection(string name, string definition)
        {
            return new Collection(AddObject(name, definition));
        }

        public Collection CreateCollection(string name)
        {
            return new Collection(AddObject(name, null));
        }

        public Element CreateElement(string name, string definition)
        {
            return new Element(AddObject(name, definition));
        }

        public Product CreateExtension(string name, string definition)
        {
            return new Product(AddObject(name, definition));
        }

        protected JObject AddObject(string name, string definition)
        {
            var json = new JObject();
            if (!string.IsNullOrEmpty(definition))
                json.Add(new JProperty(Prop.Schema, definition));

            // TODO: validate it doesn't exist already.
            var prop = new JProperty(name, json);

            container.Add(prop);

            return json;
        }
    }
}