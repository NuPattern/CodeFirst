namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ComponentConfiguration : InstanceConfiguration
    {
        private ConcurrentDictionary<string, PropertyConfiguration> properties = new ConcurrentDictionary<string, PropertyConfiguration>();
        private ConcurrentDictionary<string, AssociationPropertyConfiguration> associations = new ConcurrentDictionary<string, AssociationPropertyConfiguration>();

        public IEnumerable<PropertyConfiguration> Properties { get { return properties.Values; } }
        public IEnumerable<AssociationPropertyConfiguration> Associations { get { return associations.Values; } }

        internal ComponentConfiguration()
        {
        }

        public PropertyConfiguration Property(string name)
        {
            return properties.GetOrAdd(name, s => new PropertyConfiguration { Name = s, Cardinality = Cardinality.OneToOne });
        }

        public AssociationPropertyConfiguration Association(string name)
        {
            return associations.GetOrAdd(name, s => new AssociationPropertyConfiguration { Name = s, Cardinality = Cardinality.OneToOne });
        }

        public AssociationPropertyConfiguration CollectionAssociation(string name)
        {
            return associations.GetOrAdd(name, s => new AssociationPropertyConfiguration { Name = s, Cardinality = Cardinality.ZeroToMany });
        }

        internal void Configure(ComponentSchema schema)
        {
            // TODO: do component-specific configuration here.
            base.Configure((InstanceSchema)schema);
        }
    }
}