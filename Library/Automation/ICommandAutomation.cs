namespace NuPattern.Automation
{
    using NuPattern.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ICommandAutomation : IAutomation
    {
        void Execute();
    }
}
