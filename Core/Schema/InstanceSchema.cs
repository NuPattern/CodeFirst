namespace NuPattern.Schema
{
    using System;

    internal class InstanceSchema
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public bool IsVisible { get; set; }

        public IInstanceSchema Parent { get; set;  }
        public IPatternSchema Root { get; set;  }
    }
}