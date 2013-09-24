namespace NuPattern.Configuration
{
    using System;

    public interface ISchemaConfigurator { }

    public interface ISchemaConfigurator<TSchema, TConfiguration> : ISchemaConfigurator
    {
        void Configure(TSchema schema, TConfiguration configuration);
    }
}