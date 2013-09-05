namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    internal class Property : IProperty
    {
        private object value;

        public event EventHandler Disposed = (sender, args) => { };

        public Property(string name, Component owner)
        {
            this.Name = name;
            this.Owner = owner;
        }

        public string Name { get; private set; }

        public Component Owner { get; private set; }

        public object Value
        {
            get { return ValueHandler.Get(this); }
            set { ValueHandler.Set(this, value); }
        }

        public IPropertySchema Schema { get; set; }

        public void Delete()
        {
            Owner.DeleteProperty(this);
            Owner = null;
            Disposed(this, EventArgs.Empty);
        }

        public bool ShouldSerializeValue
        {
            get { return ValueHandler.ShouldSerialize(this); }
        }

        public override string ToString()
        {
            return Name + " = " + Value;
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
    }
}