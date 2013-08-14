namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal abstract class Component : IComponent
    {
        private JObject component;

        public Component(JObject component)
        {
            this.component = component;
            component.SetModel(this);
        }

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { component.PropertyChanging += value; }
            remove { component.PropertyChanging -= value; }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { component.PropertyChanged += value; }
            remove { component.PropertyChanged -= value; }
        }

        public string Name
        {
            get { return (string)component.GetValue("Name"); }
            set { component.Property("Name").Value = value; }
        }

        public IComponent Parent
        {
            get
            {
                return component.Parent == null ? null : component.Ancestors().OfType<JObject>().First().AsComponent();
            }
        }

        public IProduct Root
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IProperty> Properties
        {
            get { return component.Properties().Select(x => new Property(x)); }
        }

        public string Definition
        {
            get { return (string)component.GetValue("Definition"); }
        }
    }
}