namespace NuPattern.Schema
{
    using System;

    internal class PatternSchema : ContainerSchema, IPatternSchema
    {
        public IToolkit Toolkit { get; set; }
    }
}