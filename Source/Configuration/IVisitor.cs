namespace NuPattern.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a generic visitor pattern interface to implement in a visitor.
    /// </summary>
    public interface IVisitor
    {
        void Visit<T>(T value);
    }
}