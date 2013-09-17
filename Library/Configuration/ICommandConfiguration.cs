namespace NuPattern.Configuration
{
    using NuPattern.Automation;
    using System;

    public interface ICommandConfiguration
    {
        ICommandAutomationSettings CreateSettings();
    }
}
