namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;

    public class DynamicBindingFixture
    {
        [Fact]
        public void when_using_code_then_can_succinctly_express_object_binding()
        {
            var binding = Execute()
                .GenerateCode(x =>
                    x.FileName.ProvidedBy().ExpressionEvaluator("{Name}Controller") &&
                    x.TargetPath.Value("~"));


            //var binding = Bind<GenerateCodeCommand>()
            //    .With(x => x.Name, Bind<ExpressionEvaluatorValueProvider>().With(x => x.Expression = "{Name}Controller"))
            //    .With(x => x.Port, 80);

            //.With(_ => _.Name, () => "Foo");

            //Console.WriteLine(binding);
        }

        public Execute Execute()
        {
            return new Execute();
        }

        private Binding<T> Bind<T>(Expression<Func<T, bool>> initializer)
        {
            return new Binding<T>();
        }

        private IValueProvider ProvidedBy<T>(Expression<Func<T, bool>> initializer)
        {
            return null;
        }
    }

    public class Execute { }

    public class GenerateCodeCommand
    {
        public string FileName { get; set; }
        public string TargetPath { get; set; }
    }

    public static class GenerateCodeExtension
    {
        public static Binding<GenerateCodeCommand> GenerateCode(this Execute execute, Expression<Func<GenerateCodeCommand, bool>> initializer)
        {
            return new Binding<GenerateCodeCommand>();
        }
    }

    public static class ProvidedExtension
    {
        public static bool Value<T>(this T @this, T value)
        {
            return true;
        }

        public static Provided<T> ProvidedBy<T>(this T value)
        {
            return new Provided<T>();
        }

        //public static TValue ProvidedBy<TProvider>(this TValue Expression<Func<TProvider, bool>> initializer)
        //{
        //    return default(TValue);
        //}
    }

    public class Provided<T>
    {
    }

    public interface IBindingContext
    {
        Binding<T> Bind<T>();
    }

    public class Binding<T>
    {
        public Binding()
        {
            //this.Binding = new Binding { Type = typeof(T) };
        }

        public Binding<T> Where(Expression<Func<T, bool>> setup)
        {
            return this;
        }

        public T Value { get; set; }

        //public Binding Binding { get; private set; }
    }

    public class Binding
    {
        public Type Type { get; set; }
        public IList<PropertyBinding> Properties { get; set; }
    }

    public abstract class PropertyBinding
    {
        public string PropertyName { get; private set; }
        //public abstract void SetValue
    }

    public interface IPropertyValue
    {
        void SetValue(IComponentContext context, object target);
    }

    public interface IValueProvider
    {
        T Provide<T>();
    }

    public class ValueProviderAttribute : Attribute
    {
        public ValueProviderAttribute(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }
    }

    public static class ExpressionEvaluatorValueProviderExtension
    {
        [ValueProvider(typeof(ExpressionEvaluatorValueProvider))]
        public static bool ExpressionEvaluator<T>(this Provided<T> provided, string expression)
        {
            return true;
        }


    }

    public class ExpressionEvaluatorValueProvider : IValueProvider
    {
        // by convention, this Expression property is matched 
        // to the expression extension method parameter?
        public string Expression { get; set; }

        public T Provide<T>()
        {
            return default(T);
        }
    }
}