namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SchemaTypeMap
    {
        private Dictionary<Type, IComponentSchema> map = new Dictionary<Type, IComponentSchema>();

        public void AddSchema(Type componentType, IComponentSchema componentSchema)
        {
            map[componentType] = componentSchema;
        }

        public IComponentSchema FindSchema(Type componentType)
        {
            return map.Find(componentType);
        }
    }
}