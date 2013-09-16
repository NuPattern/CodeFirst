namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public abstract class InstanceConfiguration : ConfigurationBase
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool? IsVisible { get; set; }

        //public PatternConfiguration Pattern { get; }

        internal void Configure(InstanceSchema schema)
        {
            // We only set the values that we got, since 
            // different ones can be set on the schema 
            // via conventions.

            if (this.Name != null)
                schema.Name = this.Name;
            if (this.DisplayName != null)
                schema.DisplayName = this.DisplayName;
            if (this.Description != null)
                schema.Description = this.Description;
            if (this.IsVisible.HasValue)
                schema.IsVisible = this.IsVisible.Value;
        }
    }
}