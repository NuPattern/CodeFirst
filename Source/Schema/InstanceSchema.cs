namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    internal abstract class InstanceSchema : IInstanceSchema, IValidatableObject
    {
        [Required(AllowEmptyStrings = false)]
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

        public void Validate()
        {
            var validationContext = new ValidationContext(this, null, null);
            Validator.ValidateObject(this, validationContext, true);
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            OnValidate(validationContext, validationResults);

            return validationResults;
        }

        protected virtual void OnValidate(ValidationContext validationContext, ICollection<ValidationResult> validationResults)
        {
        }
    }
}