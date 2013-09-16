namespace NuPattern.Automation
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ICommandAutomationSettings : IAutomationSettings
    {
        new ICommandAutomation CreateAutomation(IComponentContext context);
    }
}
