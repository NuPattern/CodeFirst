namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal abstract class Container : Component, IContainer
    {
        public Container(Component parent)
            : base(parent)
        {
        }

        public IEnumerable<Component> Components
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Product> Extensions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Collection CreateCollection(string name, string definition)
        {
            throw new NotImplementedException();
        }

        public Collection CreateCollection(string name)
        {
            throw new NotImplementedException();
        }

        public Element CreateElement(string name, string definition)
        {
            throw new NotImplementedException();
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
    }
}