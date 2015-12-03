namespace EaiGuy.ServiceControl.Monitor
{
    using Email.Messages.Commands;
    using Email;
    using InternalCommands;
    using log4net;
    using Messages.ServiceControl;
    using Messages.ServiceControl.CustomChecks;
    using NServiceBus;
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Script.Serialization;

    public class Notifier
    {
        private IBus Bus { get; set; }
        private ServiceControlMonitorSettings settings { get; set; }
        private static readonly ILog log = LogManager.GetLogger(typeof(Notifier));

        public Notifier(IBus bus)
        {
            this.Bus = bus;
            this.settings = new ServiceControlMonitorSettings();
        }

        public void Run()
        {
            if (SchedulerHelpers.IsQuietTime(settings))
            {
                log.Debug("Skipping this run since we are in a configured quiet time.");
                return;
            }

            var cmd = new BatchAndPublishCommand();

            try
            {
                // Query ServiceControl for a count of monitored dead endpoints
                Uri basePath = new Uri(this.settings.ServiceControlApiUri);
                string endpointUri = new Uri(basePath, "api/endpoints").ToString();
                string endpointJson = UrlQueryHelper.Get(endpointUri); // for example: http://localhost:9099/api/endpoints

                // remove underscores, since the json from SC looks like this: {"host_display_name":"..."} 
                //   and the serializer is expecting {"HostDisplayName":"..."}
                endpointJson = endpointJson.Replace("_", "");
                var allEndpoints = new JavaScriptSerializer().Deserialize<EndpointsView[]>(endpointJson).ToList();

                // ignore endpoints that are missing heartbeat info (i.e. endpoints that don't have the Heartbeat plugin installed)
                var monitoredEndpoints = allEndpoints.Where(x => x.Monitored == true
                        && x.HeartbeatInformation != null).ToList();

                cmd.DeadEndpointCount = monitoredEndpoints.Count(x => 
                x.HeartbeatInformation.ReportedStatus == Messages.ServiceControl.Status.Dead);

                // Query ServiceControl for a count of failed custom checks
                string checkUri = new Uri(basePath, "api/customchecks?status=fail").ToString();
                var checkJson = UrlQueryHelper.Get(checkUri); // http://localhost:9099/api/customchecks?status=fail

                // remove underscores, since the json from SC looks like this: {"host_display_name":"..."} 
                // and the serializer is expecting {"HostDisplayName":"..."}
                checkJson = checkJson.Replace("_", "");
                CustomCheck[] failedCustomChecks = new JavaScriptSerializer().Deserialize<CustomCheck[]>(checkJson);

                cmd.FailedCheckCount = failedCustomChecks.Count();
            }
            catch (WebException e)
            {
                // send an alert email if ServiceControl is dead
                string subject = "ServiceControl Monitor: " + "Error Querying the ServiceControl REST API";
                string body = String.Format("Exception querying the ServiceControl REST API at '{0}'. \r\n"
                + "Please ensure the 'Particular ServiceControl' Windows service is running and the above URL is available,"
                + " and check the EaiGuy.ServiceControl.Monitor log file for additional error details."
                + "\r\n\r\nError: {1}", this.settings.ServiceControlApiUri, e.ToString());

                log.Info("Sending error email: " + subject);

                new EmailProvider(new EmailSettings())
                    .SendEmail(new EmailCommand(settings.ErrorEmailAddress, subject, body));
            }

            // kick off the process of batching and publishing status updates
            this.Bus.SendLocal(cmd);
        }
    }
}