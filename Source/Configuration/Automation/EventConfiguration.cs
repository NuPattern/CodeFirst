namespace NuPattern.Configuration
{
    using NetFx.StringlyTyped;
    using NuPattern.Properties;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reactive;

    public class EventConfiguration : AutomationConfiguration, IValidatableObject
    {
        [Required]
        public BindingConfiguration EventBinding { get; set; }

        // TODO: eventually, validate either Wizard or Command are specified.
        [Required]
        public BindingConfiguration CommandBinding { get; set; }
        
        //public WizardConfiguration WizardConfiguration { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!(typeof(IObservable<IEventPattern<object, EventArgs>>).IsAssignableFrom(EventBinding.Type)))
            {
                results.Add(new ValidationResult(Strings.EventAutomationSettings.EventTypeMustBeObservable(
                    Stringly.ToTypeName(typeof(IObservable<IEventPattern<object, EventArgs>>))), 
                    new[] { "EventBinding.Type" }));
            }

            // TODO: validate either Wizard or Command are specified.
            
            return results;
        }
    }
}