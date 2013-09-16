namespace NuPattern.Schema
{
    using System;

    internal class ElementSchema : ContainerSchema, IElementSchema, IElementInfo
    {
        internal ElementSchema(string schemaId)
            : base(schemaId)
        {
        }
    }
}