namespace NuPattern
{
    using System;

    /// <summary>
    /// Interface implemented by commands.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command
        /// </summary>
        void Execute();
    }
}