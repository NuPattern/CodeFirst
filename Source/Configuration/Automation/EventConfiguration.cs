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
        public Type EventType { get; set; }

        public object EventSettings { get; set; }

        // TODO: eventually, validate either Wizard or Command are specified.
        [Required]
        public ICommandConfiguration CommandConfiguration { get; set; }
        
        //public WizardConfiguration WizardConfiguration { get; set; }

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

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.Visit(this);

            CommandConfiguration.Accept(visitor);

            return visitor;
        }
    }
}