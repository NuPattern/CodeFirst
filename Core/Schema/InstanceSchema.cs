namespace NuPattern.Schema
{
    using System;
    using System.Linq;

    internal class InstanceSchema : IInstanceSchema
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public bool IsVisible { get; set; }

        public IInstanceSchema Parent { get; set; }

        public IPatternSchema Root
        {
            get
            {
                IInstanceSchema current = this;
                IPatternSchema pattern = this as IPatternSchema;
                while (current != null && pattern == null)
                {
                    current = current.Parent;
                    pattern = current as IPatternSchema;
                }

                return pattern;
            }
        }
    }
}