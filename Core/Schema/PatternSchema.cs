namespace NuPattern.Schema
{
    using System;

    internal class PatternSchema : ContainerSchema, IPatternSchema
    {
        public IToolkitSchema Toolkit { get; set; }
    }
}