namespace NuPattern.Tookit
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class Builder : ToolkitBuilder
    {
        public override IToolkitSchema Build()
        {
            this.Pattern<IAws>()
                .HasDisplayName("foo")
                .HasName("bar")
                .Property(x => x.AccessKey).Hidden();

            return base.Build();
        }
    }

    public class ToolkitBuilder
    {
        private ToolkitConfiguration configuration = new ToolkitConfiguration();

        public virtual IToolkitSchema Build()
        {
            var schema = new ToolkitSchema();

            foreach (var config in configuration.Patterns)
            {
                var pattern = new PatternSchema();
                config.Configure(pattern);

                // Here we'd apply conventions.

                schema.Patterns.Add(pattern);
            }

            return schema;
        }

        public virtual PatternConfiguration<TPattern> Pattern<TPattern>()
        {
            return new PatternConfiguration<TPattern>(configuration.Pattern(typeof(TPattern)));
        }
    }

    public interface IConvention 
    {
        void Apply(ToolkitConfiguration configuration);
    }

    public interface IConvention<TMemberInfo, TConfiguration> : IConvention
        where TMemberInfo : MemberInfo
        where TConfiguration : ConfigurationBase
    {
        void Apply(TMemberInfo memberInfo, Func<TConfiguration> configuration);
    }

    public class AwsConfiguration : PatternConfiguration<IAws>
    {
        public AwsConfiguration()
        {
            HasDisplayName("Aws")
                .Property(_ => _.AccessKey)
                .Hidden();
        }
    }

    public class ToolkitConfiguration
    {
        private ConcurrentDictionary<Type, PatternConfiguration> patternConfigurations = new ConcurrentDictionary<Type, PatternConfiguration>();

        public PatternConfiguration Pattern(Type patternType)
        {
            return patternConfigurations.GetOrAdd(patternType, t => new PatternConfiguration(t) { Name = patternType.Name });
        }

        public IEnumerable<PatternConfiguration> Patterns { get { return patternConfigurations.Values; } }
    }

    public class PatternConfiguration<TPattern>
    {
        private PatternConfiguration configuration;

        public PatternConfiguration()
            : this(new PatternConfiguration(typeof(TPattern)))
        {
        }

        internal PatternConfiguration(PatternConfiguration configuration)
        {
            this.configuration = configuration;
        }

        internal PatternConfiguration Configuration { get { return this.configuration; } }

        public PatternConfiguration<TPattern> HasName(string name)
        {
            this.configuration.Name = name;
            return this;
        }

        public PatternConfiguration<TPattern> HasDisplayName(string displayName)
        {
            this.configuration.DisplayName = displayName;
            return this;
        }

        public PropertyConfiguration<TProperty> Property<TProperty>(Expression<Func<TPattern, TProperty>> property)
        {
            return new PropertyConfiguration<TProperty>(this.configuration.Property(((MemberExpression)property.Body).Member.Name));
        }
    }

    public class PropertyConfiguration<TProperty>
    {
        private PropertyConfiguration configuration;

        internal PropertyConfiguration(PropertyConfiguration configuration)
        {
            this.configuration = configuration;
            configuration.Type = typeof(TProperty);
        }

        public PropertyConfiguration<TProperty> Hidden()
        {
            this.configuration.Hidden = true;
            return this;
        }
    }

    public class PatternConfiguration
    {
        private ConcurrentDictionary<string, PropertyConfiguration> properties = new ConcurrentDictionary<string, PropertyConfiguration>();

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<PropertyConfiguration> Properties { get { return properties.Values; } }

        public PatternConfiguration(Type patternType)
        {
            this.PatternType = patternType;
        }

        internal Type PatternType { get; private set; }

        public PropertyConfiguration Property(string name)
        {
            return properties.GetOrAdd(name, s => new PropertyConfiguration { Name = s });
        }

        internal void Configure(PatternSchema schema)
        {
            schema.Name = this.Name;
            schema.DisplayName = this.DisplayName;
        }
    }

    public class PropertyConfiguration
    {
        public string Name { get; set; }
        public bool Hidden { get; set; }
        public Type Type { get; set; }

        internal void Configure(PropertySchema schema)
        {
            schema.Name = this.Name;
            schema.Type = this.Type;
            schema.IsVisible = !this.Hidden;
        }
    }

    public abstract class ConfigurationBase
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }
    }
}