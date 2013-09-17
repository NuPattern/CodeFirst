namespace NuPattern.Configuration
{
    using NetFx.StringlyTyped;
    using NuPattern.Automation;
    using NuPattern.Configuration.Schema;
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
        public Type EventType { get; set; }

        public object EventSettings { get; set; }

        // TODO: eventually, validate either Wizard or Command are specified.
        [Required]
        public ICommandConfiguration CommandConfiguration { get; set; }
        
        //public WizardConfiguration WizardConfiguration { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Apply(IComponentSchema schema)
        {
            // Automation configuration must be valid at this point.
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);

            var commandSettings = CommandConfiguration.CreateSettings();

            var eventAutomation = new EventAutomationSettings(EventType, EventSettings, commandSettings);
            schema.AddAutomation(eventAutomation);
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (EventType != null && !(typeof(IObservable<IEventPattern<object, EventArgs>>).IsAssignableFrom(EventType)))
            {
                results.Add(new ValidationResult(Strings.EventAutomationSettings.EventTypeMustBeObservable(
                    Stringly.ToTypeName(typeof(IObservable<IEventPattern<object, EventArgs>>))), 
                    new[] { "EventType" }));
            }

            // TODO: validate either Wizard or Command are specified.
            
            return results;
        }
    }
}