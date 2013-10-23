namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class BindingConfiguration<T>
        where T : class
    {
        public BindingConfiguration()
            : this(new BindingConfiguration())
        {
        }

        public BindingConfiguration(BindingConfiguration configuration)
        {
            configuration.Type = typeof(T);

            this.Configuration = configuration;
        }

        public BindingConfiguration<T> Property(Expression<Func<T, object>> propertyExpression, object value)
        {
            var propertyName = Reflect<T>.GetPropertyName(propertyExpression);

            Configuration.Property(propertyName, value);

            return this;
        }

        public BindingConfiguration<T> Property(Expression<Func<T, object>> propertyExpression, BindingConfiguration valueProvider)
        {
            var propertyName = Reflect<T>.GetPropertyName(propertyExpression);

            Configuration.Property(propertyName, valueProvider);

            return this;
        }

        public BindingConfiguration Configuration { get; private set; }
    }
}