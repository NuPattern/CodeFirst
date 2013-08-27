namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    internal abstract class InstanceSchema : IInstanceSchema
    {
        protected InstanceSchema(string name)
        {
            Guard.NotNullOrEmpty(() => name, name);

            this.Name = name;

            this.IsVisible = true;
        }

        public string Name { get; set; }

        public string DisplayName { get; set; }
        
        public string Description { get; set; }

        public bool IsVisible { get; set; }

        public IInstanceSchema Parent { get; set; }

        public IProductSchema Root
        {
            get
            {
                IInstanceSchema current = this;
                IProductSchema pattern = this as IProductSchema;
                while (current != null && pattern == null)
                {
                    current = current.Parent;
                    pattern = current as IProductSchema;
                }

                return pattern;
            }
        }
    }
}