namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class ToolkitConfiguration
    {
        private readonly ConcurrentDictionary<object, PatternConfiguration> patternConfigurations = new ConcurrentDictionary<object, PatternConfiguration>();
        private readonly ConcurrentDictionary<object, CollectionConfiguration> collectionConfigurations = new ConcurrentDictionary<object, CollectionConfiguration>();
        private readonly ConcurrentDictionary<object, ElementConfiguration> elementConfigurations = new ConcurrentDictionary<object, ElementConfiguration>();

        public PatternConfiguration Pattern(Type patternType)
        {
            return patternConfigurations.GetOrAdd(patternType, t => new PatternConfiguration(t) { Name = patternType.Name });
        }

        public CollectionConfiguration Collection(Type collectionType)
        {
            return collectionConfigurations.GetOrAdd(collectionType, t => new CollectionConfiguration(t) { Name = collectionType.Name });
        }

        public ElementConfiguration Element(Type elementType)
        {
            return elementConfigurations.GetOrAdd(elementType, t => new ElementConfiguration(t) { Name = elementType.Name });
        }

        public IEnumerable<PatternConfiguration> Patterns { get { return patternConfigurations.Values; } }
        public IEnumerable<CollectionConfiguration> Collections { get { return collectionConfigurations.Values; } }
        public IEnumerable<ElementConfiguration> Elements { get { return elementConfigurations.Values; } }

        internal void Add(PatternConfiguration configuration)
        {
            var keyed = (IKeyedConfiguration)configuration;
            if (patternConfigurations.ContainsKey(keyed.Key))
                throw new ArgumentException("Configuration for pattern type " + keyed.Key + " already exists.");

            patternConfigurations[keyed.Key] = configuration;
        }

        internal void Add(CollectionConfiguration configuration)
        {
            var keyed = (IKeyedConfiguration)configuration;
            if (collectionConfigurations.ContainsKey(keyed.Key))
                throw new ArgumentException("Configuration for collection type " + keyed.Key + " already exists.");

            collectionConfigurations[keyed.Key] = configuration;
        }

        internal void Add(ElementConfiguration configuration)
        {
            var keyed = (IKeyedConfiguration)configuration;
            if (elementConfigurations.ContainsKey(keyed.Key))
                throw new ArgumentException("Configuration for element type " + keyed.Key + " already exists.");

            elementConfigurations[keyed.Key] = configuration;
        }
    }
}