namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    public class AnonymousCommandConfiguration<T> : ICommandConfiguration
        where T : class
    {
        private Action<T> command;

        public AnonymousCommandConfiguration(Action<T> command)
        {
            this.command = command;
        }
    }
}