namespace NuPattern.Configuration
{
    using NuPattern.Commands;
    using System;
    using System.ComponentModel;
    using System.Linq;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TraceMessageCommandExtension
    {
        public static BindingConfiguration TraceMessage(this CommandFor configuration, string message)
        {
            // TODO: setting the entry directly allows to have generics-based configuration, which will be easier to update later.
            // The problem is how to deal with replace vs add behavior: when setting, should we remove the existing config 
            // from the component? should we just leave it there? Maybe we should always add, and upon building the actual 
            // settings/schema object, we only bring over the ones that are used/referenced?
            //configuration.Configuration = new CommandConfiguration<TraceMessageCommand, TraceMessageSettings>(new TraceMessageSettings(message));

            return new BindingConfiguration<TraceMessageCommand>(configuration.Configuration)
                .Property(x => x.Message, message)
                .Configuration;
        }

        public static BindingConfiguration TraceMessage(this CommandFor configuration, BindingConfiguration messageProvider)
        {
            // TODO: setting the entry directly allows to have generics-based configuration, which will be easier to update later.
            // The problem is how to deal with replace vs add behavior: when setting, should we remove the existing config 
            // from the component? should we just leave it there? Maybe we should always add, and upon building the actual 
            // settings/schema object, we only bring over the ones that are used/referenced?
            //configuration.Configuration = new CommandConfiguration<TraceMessageCommand, TraceMessageSettings>(new TraceMessageSettings(message));

            return new BindingConfiguration<TraceMessageCommand>(configuration.Configuration)
                .Property(x => x.Message, messageProvider)
                .Configuration;
        }
    }
}