namespace NuPattern.Automation
{
    using System;

    public interface IAutomationSettingsFactory<TConfiguration, TSettings>
    {
        TSettings CreateSettings(TConfiguration configuration);
    }
}