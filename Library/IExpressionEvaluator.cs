namespace NuPattern
{
    /// <summary>
    /// Defines an expression evaluator.
    /// </summary>
    public interface IExpressionEvaluator
    {
        /// <summary>
        /// Evaluates the specified expression.
        /// </summary>
        string Evaluate(string expression);
    }
}
