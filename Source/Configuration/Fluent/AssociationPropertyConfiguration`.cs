namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class AssociationPropertyConfiguration<TProperty> : ConfigurationBase
    {
        private AssociationPropertyConfiguration configuration;

        internal AssociationPropertyConfiguration(AssociationPropertyConfiguration configuration)
        {
            this.configuration = configuration;
            configuration.PropertyType = typeof(TProperty);
        }

        public AssociationPropertyConfiguration<TProperty> AllowAddNew()
        {
            this.configuration.AllowAddNew = true;
            return this;
        }

        public AssociationPropertyConfiguration<TProperty> AllowDelete()
        {
            this.configuration.AllowDelete = true;
            return this;
        }

        public AssociationPropertyConfiguration<TProperty> AutoCreate()
        {
            this.configuration.AutoCreate  = true;
            return this;
        }

        public AssociationPropertyConfiguration<TProperty> Hidden()
        {
            this.configuration.Hidden = true;
            return this;
        }

        public AssociationPropertyConfiguration<TProperty> Optional()
        {
            if (this.configuration.Cardinality.GetValueOrDefault() == Cardinality.OneToOne)
                this.configuration.Cardinality = Cardinality.ZeroToOne;
            else if (this.configuration.Cardinality.GetValueOrDefault() == Cardinality.OneToMany)
                this.configuration.Cardinality = Cardinality.ZeroToMany;

            return this;
        }
    }
}