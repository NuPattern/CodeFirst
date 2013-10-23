namespace NuPattern.Tookit.Simple
{
    using NuPattern.Configuration;
    using NuPattern.Schema;
    using System;
    using System.Linq;

    public class SimpleBuilder : ToolkitBuilder
    {
        public SimpleBuilder()
            : base("Simple", "1.0")
        {
            var commandConfig = this
                .Command()
                .TraceMessage(
                    this.ProvidedBy().Expression("Product: {Name} (AccessKey: {AccessKey}, SecretKey: {SecretKey})"));

            this.Product<IAmazonWebServices>()
                .OnEvent()
                .PropertyChanged(aws => aws.AccessKey)
                .Execute(commandConfig);

            this.Product<IAmazonWebServices>()
                .OnEvent()
                .PropertyChanged(aws => aws.SecretKey)
                .Execute(commandConfig);
        }
    }
}