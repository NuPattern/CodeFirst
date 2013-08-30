namespace NuPattern.Schema
{
    using System;

    internal class ElementSchema : ContainerSchema, IElementSchema
    {
        /// <summary>
        /// Internal constructor used by tests to allow for easy 
        /// functional construction.
        /// </summary>
        internal ElementSchema(string schemaId)
            : this(schemaId, null)
        {
        }

        public ElementSchema(string schemaId, ComponentSchema parentSchema)
            : base(schemaId, parentSchema)
        {
        }
    }
}