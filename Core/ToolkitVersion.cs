namespace NuPattern
{
    using System;
    using System.Linq;

    public class ToolkitVersion : IToolkitVersion
    {
        public string Id { get; set; }

        public SemanticVersion Version { get; set; }
    }
}