namespace NuPattern
{
    using System;
    using System.Linq;

    public class ToolkitIdentifier : IToolkitIdentifier
    {
        public string Id { get; set; }

        public SemanticVersion Version { get; set; }
    }
}