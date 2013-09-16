namespace NuPattern.Automation
{
    using System;
    using System.Linq;

    public class ConfigurableReference
    {
        public ConfigurableReference(Type referenceType, object settings = null)
        {
            Guard.NotNull(() => referenceType, referenceType);

            this.ReferenceType = referenceType;
            this.Settings = settings;
        }

        public Type ReferenceType { get; private set; }
        public object Settings { get; private set; }
    }
}