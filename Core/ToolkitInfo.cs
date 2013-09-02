namespace NuPattern
{
    using System;
    using System.Linq;

    public class ToolkitInfo : IToolkitInfo
    {
        public string Id { get; set; }

        public SemanticVersion Version { get; set; }
    }
}