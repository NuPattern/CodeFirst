namespace NuPattern.ValueProviders
{
    using System;
    using System.Linq;

    public class ExpressionProvider : IValueProvider
    {
        public ExpressionProvider(IComponent context)
        {
            this.Context = context;
        }

        public IComponent Context { get; private set; }

        public string Expression { get; set; }

        public object GetValue()
        {
            return ExpressionEvaluator.DefaultFactory(this.Context).Evaluate(this.Expression);
        }
    }
}