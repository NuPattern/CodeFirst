namespace NuPattern
{
    using NuPattern.Schema;
    using System;
    using System.Linq;
    using Xunit;

    public class ReflectionSchemaBuilderFixture
    {
        [Fact]
        public void when_building_schema_then_loads_built_patterns()
        {
            var schema = new ReflectionSchemaBuilder().Build("AWS", "1.0.0", this.GetType().Assembly);

            Assert.NotNull(schema);
        }
    }
}