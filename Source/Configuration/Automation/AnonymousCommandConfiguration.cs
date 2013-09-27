namespace NuPattern.Configuration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public abstract class AnonymousCommandConfiguration : ICommandConfiguration
    {
        public AnonymousCommandConfiguration(Delegate command)
        {
            Guard.NotNull(() => command, command);

            this.Command = command;
        }

        [Required]
        public abstract Type ArgumentType { get; }
        
        [Required]
        public Delegate Command { get; private set; }

        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IVisitor
        {
            visitor.Visit(this);
            return visitor;
        }
    }

    public class AnonymousCommandConfiguration<T> : AnonymousCommandConfiguration
        where T : class
    {
        public AnonymousCommandConfiguration(Action<T> command)
            : base(command)
        {
        }

        public override Type ArgumentType
        {
            get { return typeof(T); }
        }
    }
}