namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class PropertyConfiguration : InstanceConfiguration
    {
        public Cardinality? Cardinality { get; set; }
        public bool? Hidden { get; set; }
        public Type PropertyType { get; set; }

        internal virtual void Configure(PropertySchema schema)
        {
            base.Configure((InstanceSchema)schema);

            if (this.PropertyType != null)
                schema.Type = this.PropertyType;
            if (this.Hidden.HasValue)
                schema.IsVisible = !this.Hidden.Value;
        }
    }
}