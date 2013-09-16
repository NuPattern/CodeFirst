namespace NuPattern
{
    using NuPattern.Configuration;
    using NuPattern.Schema;
    using NuPattern.Tookit.Simple;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;
    using System.Reactive;
    using System.Reactive.Linq;

    public class ToolkitBuilderFixture
    {
        [Fact]
        public void when_building_toolkit_then_can_specify_event_automation()
        {
            var builder = new ToolkitBuilder("Simple", "1.0");

            builder.Product<IAmazonWebServices>()
                .On()
                .PropertyChanged(aws => aws.AccessKey)
                // TODO: Wizard
                .Execute()
                .TraceMessage("Access Key Changed");

            // Runtime would call this builder when solution/project is opened:
            var schema = builder.Build();
            var rootContext = new ComponentContext();

            // User instantiates a product via Solution Builder:
            var product = new Product("MyWebService", "IAmazonwebServices");
            ComponentMapper.SyncProduct(product, schema.ProductSchemas.First());

            var productContext = rootContext.BeginScope(b => b.RegisterInstance(product));

            foreach (var setting in schema.ProductSchemas.First().AutomationSettings)
            {
                product.AddAutomation(setting.CreateAutomation(productContext));
            }

            // User changes a property via property browser:
            product.Set("AccessKey", "asdf");
        }
    }

    public static class ProductConfigurationExtensions
    {
        public static EventsFor<T> On<T>(this ProductConfiguration<T> configuration)
            where T : class
        {
            return new EventsFor<T>(configuration.Configuration);
        }
    }

    public class EventsFor<T> where T : class
    {
        private ComponentConfiguration parent;

        internal EventsFor(ComponentConfiguration parent)
        {
            this.parent = parent;
        }

        internal ComponentConfiguration Configuration { get { return parent; } }
    }

    public class EventConfiguration<T> where T : class
    {
        private ComponentConfiguration parent;
        private EventConfiguration configuration;

        internal EventConfiguration(ComponentConfiguration parent, EventConfiguration configuration)
        {
            this.parent = parent;
            this.configuration = configuration;
        }

        public void Execute(Action<T> action)
        {
            //parent.Automations.Add()
        }

        public CommandsFor<T> Execute()
        {
            if (configuration.CommandConfiguration == null)
            {
                configuration.CommandConfiguration = new CommandConfiguration();
            }

            return new CommandsFor<T>(configuration.CommandConfiguration);
        }
    }

    public class CommandsFor<T> where T : class
    {
        internal CommandsFor(CommandConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public CommandConfiguration Configuration { get; private set; }
    }

    public static class TraceMessageExtension
    {
        public static void TraceMessage<T>(this CommandsFor<T> configuration, string message)
            where T : class
        {
            // TODO: setting the entry directly allows to have generic configuration, which will be easier to update later.
            // The problem is how to deal with replace vs add behavior: when setting, should we remove the existing config 
            // from the component? should we just leave it there? Maybe we should always add, and upon building the actual 
            // settings/schema object, we only bring over the ones that are used/referenced?
            //configuration.Configuration = new CommandConfiguration<TraceMessageCommand, TraceMessageSettings>(new TraceMessageSettings(message));

            configuration.Configuration.CommandType = typeof(TraceMessageCommand);
            configuration.Configuration.CommandSettings = new TraceMessageSettings(message);
        }
    }

    public class TraceMessageSettings
    {
        public TraceMessageSettings(string message)
        {
            this.Message = message;
        }

        [Required(AllowEmptyStrings = false)]
        public string Message { get; set; }
    }

    public class TraceMessageCommand : ICommand
    {
        private TraceMessageSettings settings;

        public TraceMessageCommand(TraceMessageSettings settings)
        {
            this.settings = settings;
        }

        public void Execute()
        {
            System.Diagnostics.Trace.WriteLine(settings.Message);
        }
    }

    public static class OnPropertyChanged
    {
        public static EventConfiguration<T> PropertyChanged<T>(this EventsFor<T> configuration, Expression<Func<T, object>> propertyExpression)
            where T : class
        {
            return PropertyChanged(configuration, ((MemberExpression)propertyExpression.Body).Member.Name);
        }

        public static EventConfiguration<T> PropertyChanged<T>(this EventsFor<T> configuration, string propertyName)
            where T : class
        {
            var eventConfig = new EventConfiguration
            {
                EventType = typeof(OnPropertyChangedEvent),
                EventSettings = new OnPropertyChangedEventSettings { PropertyName = propertyName },
            };

            configuration.Configuration.Automations.Add(eventConfig);

            return new EventConfiguration<T>(configuration.Configuration, eventConfig);
        }
    }

    public class OnPropertyChangedEvent : IObservable<IEventPattern<object, EventArgs>>
    {
        private IObservable<IEventPattern<object, EventArgs>> eventSource;

        public OnPropertyChangedEvent(IComponent component, OnPropertyChangedEventSettings settings)
        {
            // Setup event observer, filter according to settings.PropertyName
            eventSource = Observable.FromEventPattern<PropertyChangedEventArgs>(
                handler => component.PropertyChanged += handler, 
                handler => component.PropertyChanged -= handler)
                .Where(e => e.EventArgs.PropertyName == settings.PropertyName);
        }

        public IDisposable Subscribe(IObserver<IEventPattern<object, EventArgs>> observer)
        {
            return eventSource.Subscribe(observer);
        }
    }

    public class OnPropertyChangedEventSettings
    {
        public string PropertyName { get; set; }
    }
}