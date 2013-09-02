namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    internal class Property : IProperty
    {
        private object value;

        public Property(string name, Component owner)
        {
            this.Name = name;
            this.Owner = owner;
        }

        public string Name { get; private set; }

        public Component Owner { get; private set; }

        object IProperty.Value
        {
            get { return ValueHandler.Get(this); }
            set { ValueHandler.Set(this, value); }
        }

        public IPropertySchema Schema { get; set; }

        public void Delete()
        {
            Owner.DeleteProperty(this);
        }

        internal object GetValue()
        {
            return this.value;
        }

        internal void SetValue(object value)
        {
            this.value = value;
        }

        IComponent IProperty.Owner { get { return Owner; } }


        public bool ShouldSerializeValue
        {
            get { throw new NotImplementedException(); }
        }
    }
}