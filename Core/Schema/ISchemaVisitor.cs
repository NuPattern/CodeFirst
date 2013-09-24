namespace NuPattern.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ISchemaVisitor
    {
        void Visit<TSchema>(TSchema schema);
    }
}