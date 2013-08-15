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

        public string Definition
        {
            get { return component.Get(() => Definition); }
        }

        public string Name
        {
            get { return component.Get(() => Name); }
            set { component.Set(() => Name, value); }
        }

        public IEnumerable<IProperty> Properties
        {
            get { return component.Properties().Select(x => new Property(x)); }
        }

        public IComponent Parent
        {
            get
            {
                return component.Parent == null ? null : component.Ancestors().OfType<JObject>().Select(x => x.AsComponent()).FirstOrDefault();
            }
        }

        public IProduct Root
        {
            get { throw new NotImplementedException(); }
        }
    }
}