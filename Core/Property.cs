namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Property : IProperty, ILineInfo
    {
        private object value;
        private object annotations;

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
            return value;
        }

        internal void SetValue(object value)
        {
            this.value = value;
        }

        #region ILineInfo

        public bool HasLineInfo { get { return LinePosition.HasValue && LineNumber.HasValue; } }

        public int? LinePosition { get; private set; }

        public int? LineNumber { get; private set; }

        internal void SetLineInfo(int lineNumber, int linePosition)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        #endregion

        #region Annotations

        public void AddAnnotation(object annotation)
        {
            Annotator.AddAnnotation(ref annotations, annotation);
        }

        public object Annotation(Type type)
        {
            return Annotator.Annotation(annotations, type);
        }

        public IEnumerable<object> Annotations(Type type)
        {
            return Annotator.Annotations(annotations, type);
        }

        public void RemoveAnnotations(Type type)
        {
            Annotator.RemoveAnnotations(ref annotations, type);
        }

        #endregion

        IComponent IProperty.Owner { get { return Owner; } }
    }
}