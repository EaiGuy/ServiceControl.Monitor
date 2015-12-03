namespace EaiGuy.ServiceControl.Monitor.Emailer
{
    using NServiceBus;
    using NServiceBus.Log4Net;
    using NServiceBus.Logging;
    using NServiceBus.Persistence;
    using RazorEngine.Templating;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            log4net.Config.XmlConfigurator.Configure();
            LogManager.Use<Log4NetFactory>();

            // configure persistence
            configuration.UsePersistence<RavenDBPersistence>();

            configuration.DisableFeature<NServiceBus.Features.Sagas>();

            // use json serializer to be compatible with MessageFailed messages coming from ServiceControl; this means we can't send any messages to any other ESB services, since they use XML serialization
            configuration.UseSerialization<JsonSerializer>();

            var conventionsBuilder = configuration.Conventions();
            conventionsBuilder.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands") && t.Namespace.StartsWith("EaiGuy"));
            conventionsBuilder.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events") && t.Namespace.StartsWith("EaiGuy"));
            //conventionsBuilder.DefiningEventsAs(t => t.Namespace == "ServiceControl.Contracts");
            conventionsBuilder.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages") && t.Namespace.StartsWith("EaiGuy"));

            // register the Razor template service as a single instance  http://mehdi.me/generating-html-emails-with-razorengine-part-03-caching-vs-integration-namespace-config/
            configuration.RegisterComponents(c =>
            {
                //c.ConfigureComponent<IRazorEngineService>(x => new RazorEngineService(), DependencyLifecycle.SingleInstance);
                c.ConfigureComponent(x => new TemplateService(), DependencyLifecycle.SingleInstance);
            });
        }
    }
}
