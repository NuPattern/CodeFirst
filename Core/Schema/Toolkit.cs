namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    internal class Toolkit : IToolkit
    {
        public Toolkit()
        {
            var patterns = new ObservableCollection<PatternSchema>();
            patterns.CollectionChanged += OnPatternsChanged;
            this.Patterns = patterns;
        }

        public string Id { get; set; }
        public string Version { get; set; }
        IEnumerable<IPatternSchema> IToolkit.Patterns { get { return Patterns; }}

        public ICollection<PatternSchema> Patterns { get; private set; }

        private void OnPatternsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var pattern in e.NewItems.OfType<PatternSchema>())
                {
                    pattern.Toolkit = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var pattern in e.OldItems.OfType<PatternSchema>())
                {
                    pattern.Toolkit = null;
                }
            }
        }
    }
}