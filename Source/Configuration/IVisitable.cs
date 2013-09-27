namespace NuPattern.Configuration
{
    using System;
    using System.Linq;

    /// <summary>
    /// Provides a generic visitor pattern interface to implement in a model.
    /// </summary>
    public interface IVisitable
    {
        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor, inferred from the passed-in <paramref name="visitor"/>.</typeparam>
        /// <param name="visitor">The visitor instance to accept.</param>
        /// <returns>The received visitor. Allows for easy collecting of the results from the visitor.</returns>
        TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IVisitor;
    }
}