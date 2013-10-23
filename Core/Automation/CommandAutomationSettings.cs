namespace NuPattern.Automation
{
    using NuPattern.Configuration;
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandAutomationSettings : ICommandAutomationSettings
    {
        private object annotations;

        public CommandAutomationSettings(BindingConfiguration binding)
        {
            Guard.NotNull(() => binding, binding);

            if (!typeof(ICommand).IsAssignableFrom(binding.Type))
                throw new ArgumentException();

            this.Binding = binding;
        }

        public BindingConfiguration Binding { get; private set; }

        #region Annotations

        public void AddAnnotation(object annotation)
        {
            Annotator.AddAnnotation(ref annotations, annotation);
        }

        public object Annotation(Type type)
        {
            return Annotator.Annotation(annotations, type);
        }

        public IEnumerable<object> Annotations(Type type)
        {
            return Annotator.Annotations(annotations, type);
        }

        public void RemoveAnnotations(Type type)
        {
            Annotator.RemoveAnnotations(ref annotations, type);
        }

        #endregion

        public CommandAutomation CreateAutomation(IComponentContext context)
        {
            return new CommandAutomation(context, this);
        }

        IAutomation IAutomationSettings.CreateAutomation(IComponentContext context)
        {
            return CreateAutomation(context);
        }

        ICommandAutomation ICommandAutomationSettings.CreateAutomation(IComponentContext context)
        {
            return CreateAutomation(context);
        }
    }
}