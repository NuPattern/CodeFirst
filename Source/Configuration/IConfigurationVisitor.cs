namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IConfigurationVisitor
    {
        void Visit<TConfiguration>(TConfiguration configuration);
    }
}