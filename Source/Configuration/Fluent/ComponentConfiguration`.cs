namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class ComponentConfiguration<TComponent> : ConfigurationBase
    {
        private ComponentConfiguration configuration;

        protected ComponentConfiguration(ComponentConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public PropertyConfiguration<TProperty> Property<TProperty>(Expression<Func<TComponent, TProperty>> property)
        {
            return new PropertyConfiguration<TProperty>(this.configuration.Property(((MemberExpression)property.Body).Member.Name));
        }

        public AssociationPropertyConfiguration<TTarget> Association<TTarget>(Expression<Func<TComponent, TTarget>> property)
        {
            return new AssociationPropertyConfiguration<TTarget>(this.configuration.Association(((MemberExpression)property.Body).Member.Name));
        }

        public AssociationPropertyConfiguration<TTarget> CollectionAssociation<TTarget>(Expression<Func<TComponent, IEnumerable<TTarget>>> property)
        {
            return new AssociationPropertyConfiguration<TTarget>(this.configuration.CollectionAssociation(((MemberExpression)property.Body).Member.Name));
        }
    }
}