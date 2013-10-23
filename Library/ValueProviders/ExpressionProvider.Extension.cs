namespace NuPattern.Configuration
{
    using NuPattern.ValueProviders;
    using System;

    public static class ExpressionProviderExtension 
    {
        public static BindingConfiguration Expression(this ProvidedBy provided, string expression)
        {
            return new BindingConfiguration<ExpressionProvider>()
                .Property(x => x.Expression, expression)
                .Configuration;
        }

        public static BindingConfiguration Expression(this ProvidedBy provided, BindingConfiguration providedExpression)
        {
            return new BindingConfiguration<ExpressionProvider>()
                .Property(x => x.Expression, providedExpression)
                .Configuration;
        }
    }
}