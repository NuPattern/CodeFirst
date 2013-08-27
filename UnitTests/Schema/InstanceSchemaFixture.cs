namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Xunit;

    public class InstanceSchemaFixture
    {
        [Fact]
        public void when_validating_then_validates_instance_name_property()
        {
            var schema = new ConcreteSchema { Id = 23 };

            var ex = Assert.Throws<ValidationException>(() => schema.Validate());

            Assert.Equal("Name", ex.ValidationResult.MemberNames.First());
        }

        [Fact]
        public void when_validating_then_validates_derived_properties()
        {
            var schema = new ConcreteSchema { Name = "Foo" };

            var ex = Assert.Throws<ValidationException>(() => schema.Validate());

            Assert.Equal("Id", ex.ValidationResult.MemberNames.First());
        }

        [Fact]
        public void when_validating_then_invokes_derived_validation()
        {
            var schema = new ConcreteSchema { Name = "Foo", Id = 23 };

            var ex = Assert.Throws<ValidationException>(() => schema.Validate());

            Assert.Equal("Bar", ex.ValidationResult.MemberNames.First());
        }

        [Fact]
        public void when_validating_then_succeeds()
        {
            var schema = new ValidatingSchema { Name = "Foo", Id = 23 };

            schema.Validate();
        }

        private class ValidatingSchema : InstanceSchema
        {
            [Required]
            public int? Id { get; set; }
        }

        private class ConcreteSchema : InstanceSchema
        {
            [Required]
            public int? Id { get; set; }

            protected override void OnValidate(ValidationContext validationContext, ICollection<ValidationResult> validationResults)
            {
                base.OnValidate(validationContext, validationResults);
                validationResults.Add(new ValidationResult("Bar is missing", new[] { "Bar" }));
            }
        }
    }
}