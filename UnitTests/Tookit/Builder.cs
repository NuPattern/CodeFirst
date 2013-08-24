namespace NuPattern.Tookit
{
    using NuPattern.Configuration;
    using NuPattern.Schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class Builder : ToolkitBuilder
    {
        public override IToolkitSchema Build()
        {
            this.Pattern<IAws>()
                .HasDisplayName("foo")
                .HasName("bar")
                .Property(x => x.AccessKey).Hidden();

            return base.Build();
        }
    }


    public class AwsConfiguration : PatternConfiguration<IAws>
    {
        public AwsConfiguration()
        {
            HasDisplayName("Aws")
                .Property(_ => _.AccessKey)
                .Hidden();
        }
    }

}