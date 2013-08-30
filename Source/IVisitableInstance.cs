namespace NuPattern
{
    using System;
    using System.Linq;

    /// <summary>
    /// Provides visitor pattern support for the runtime model.
    /// </summary>
    public interface IVisitableInstance
    {
        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor, inferred from the passed-in <paramref name="visitor"/>.</typeparam>
        /// <param name="visitor">The visitor instance to accept.</param>
        /// <returns>The received visitor. Allows for easy collecting of the results from the visitor.</returns>
        TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : InstanceVisitor;
    }
}