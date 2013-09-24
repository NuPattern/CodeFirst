namespace NuPattern.Schema
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public interface IAutomationSettings : IAnnotated
    {
        /// <summary>
        /// Creates the runtime automation for this settings element.
        /// </summary>
        IAutomation CreateAutomation(IComponentContext context);
    }
}