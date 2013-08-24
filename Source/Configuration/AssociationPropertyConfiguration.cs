namespace NuPattern.Configuration
{
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class AssociationPropertyConfiguration : PropertyConfiguration
    {
        public bool? AllowAddNew { get; set; }
        public bool? AllowDelete { get; set; }
        public bool? AutoCreate { get; set; }

        internal override void Configure(PropertySchema schema)
        {
            base.Configure(schema);
            // TODO: asociation property configuration here.
        }
    }
}