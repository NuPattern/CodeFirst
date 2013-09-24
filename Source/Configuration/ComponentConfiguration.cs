namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    public abstract class ComponentConfiguration : IVisitableConfiguration, IAnnotated
    {
        private object annotations;

        internal ComponentConfiguration(Type componentType)
        {
            this.ComponentType = componentType;

            var automations = new ObservableCollection<AutomationConfiguration>();
            automations.CollectionChanged += OnAutomationsChanged;
            this.Automations = automations;

            var properties = new ObservableCollection<PropertyConfiguration>();
            properties.CollectionChanged += OnPropertiesChanged;
            this.Properties = properties;
        }

        public ICollection<AutomationConfiguration> Automations { get; private set; }

        public ICollection<PropertyConfiguration> Properties { get; private set; }

        public Type ComponentType { get; private set; }

        public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IConfigurationVisitor
        {
            visitor.Visit(this);

            foreach (var automation in Automations)
            {
                automation.Accept(visitor);
            }

            foreach (var property in Properties)
            {
                property.Accept(visitor);
            }

            return visitor;
        }

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

        private void OnAutomationsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var automation in e.NewItems.OfType<AutomationConfiguration>())
                {
                    automation.ComponentType = ComponentType;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var automation in e.OldItems.OfType<AutomationConfiguration>())
                {
                    automation.ComponentType = null;
                }
            }
        }

        private void OnPropertiesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var property in e.NewItems.OfType<PropertyConfiguration>())
                {
                    property.ComponentType = ComponentType;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var property in e.OldItems.OfType<PropertyConfiguration>())
                {
                    property.ComponentType = ComponentType;
                }
            }
        }
    }
}