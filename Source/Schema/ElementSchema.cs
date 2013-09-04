namespace NuPattern.Schema
{
    using System;

    internal class ElementSchema : ContainerSchema, IElementSchema
    {
        internal ElementSchema(string schemaId)
            : base(schemaId)
        {
        }
    }
}