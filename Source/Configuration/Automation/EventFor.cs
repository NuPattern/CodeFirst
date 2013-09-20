namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class EventFor<T> where T : class
    {
        private ComponentConfiguration parent;

        internal EventFor(ComponentConfiguration parent)
        {
            this.parent = parent;
        }

        public ComponentConfiguration Configuration { get { return parent; } }
    }
}