namespace NuPattern.Sample
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;

    public class SampleToolkitConfiguration
    {
    }

    public class SampleToolkit
    {
        public void Do()
        {
            IBuilder b = null;

            var prod = b.Product<IApp>();

            prod
                .On()
                    .Initialized()
                    .UnfoldTemplate();

            prod.On().Initialized().UnfoldTemplate();
            prod.On().PropertyChanged(a => a.Url).UnfoldTemplate();


            // b.On().PropertyChanged("foo");

            //b.Execute().UnfoldTemplate(....);


        }
    }

    public class ToolkitBuilder : IBuilder
    {
        public ToolkitBuilder()
        {

        }

        public IBuilder<T> Product<T>()
        {
            throw new NotImplementedException();
        }
    }

    public class AppBuilder : IBuilder<IApp>
    {
        public AppBuilder()
        {

        }

        public IOn<IApp> On()
        {
            throw new NotImplementedException();
        }

        public IBuilder<E> Element<E>()
        {
            throw new NotImplementedException();
        }
    }

    public class NonEmptyPublishValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }
    }

    //[Icon("foo.png")]
    [NonEmptyPublishValidation]
    public interface IApp
    {
        event EventHandler Restarted;

        [Description]
        [DisplayName("Balh")]
        string Url { get; set; }

        //[AutoCreate]
        //[Nullable]
        [Required]
        [Browsable(false)]
        IPublishWizard Settings { get; set; }
    }

    public interface IPublishWizard
    {
    }

    public static class OnExtensions
    {
        public static IExec<T> Initialized<T>(this IOn<T> on)
        {
            return null;
        }

        public static IExec<T> PropertyChanged<T>(this IOn<T> on, Expression<Func<T, object>> property)
        {
            return null;
        }

        public static IBuilder<T> UnfoldTemplate<T>(this IExec<T> exec)
        {
            return (IBuilder<T>)exec;
        }
    }

    public interface IBuilder
    {
        IBuilder<T> Product<T>();
    }

    public interface IBuilder<T>
    {
        IOn<T> On();

        IBuilder<E> Element<E>();
    }

    public interface IOn<T>
    {
        IExec<T> Execute();
    }

    public interface IExec<T>
    {
    }
}