namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    internal class Property : IProperty, ILineInfo
    {
        private object value;

        public event EventHandler Deleted = (sender, args) => { };

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

        public IPropertyInfo Schema { get; internal set; }

        public bool HasLineInfo { get { return LinePosition.HasValue && LineNumber.HasValue; } }

        public int? LinePosition { get; private set; }

        public int? LineNumber { get; private set; }

        public void Delete()
        {
            Owner.DeleteProperty(this);
            Owner = null;
            Deleted(this, EventArgs.Empty);
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

        internal void SetLineInfo(int lineNumber, int linePosition)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        IComponent IProperty.Owner { get { return Owner; } }
    }
}