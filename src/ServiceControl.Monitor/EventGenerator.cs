namespace EaiGuy.ServiceControl.Monitor
{
    using MonitorSaga;
    using InternalCommands;
    using Messages.Events;
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Linq;

    class EventGenerator
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EventGenerator));
        private static ServiceControlMonitorSettings settings = new ServiceControlMonitorSettings();

        public static IHealthStatusChanged GenerateEventMsg(BatchAndPublishCommand message, MonitorSagaData sagaData, IMessageCreator messageCreator)
        {
            var updateMsg = messageCreator.CreateInstance<IHealthStatusChanged>();

            updateMsg.FailedMessages = sagaData.FailedMessages.Take(settings.MaxFailedMessagesToIncludeInEvent).ToList();
            updateMsg.FailedMsgStatus = sagaData.FailedMessages.Any() ? sagaData.FailedMessages.Count().ToString() + " New Failed Message"
                + (sagaData.FailedMessages.Count() > 1 ? "s" : "") : String.Empty;

            // find endpoints that show in the list of current dead endpoints in the message, but that weren't already in the saga data's list of dead endpoints
            updateMsg.EndpointsThatJustDied = sagaData.DeadEndpoints.Where(x => sagaData.PreviouslyDeadEndpoints == null 
                || !sagaData.PreviouslyDeadEndpoints.Select(y => y.EndpointName).Contains(x.EndpointName)).ToList();

            // find endpoints that show in saga data state as being dead, but that weren't included in the list of current dead endpoints in the message
            updateMsg.EndpointsThatJustBecameAlive = sagaData.RestoredEndpoints.Where(x => sagaData.PreviouslyRestoredEndpoints == null
                || !sagaData.PreviouslyRestoredEndpoints.Select(y => y.EndpointName).Contains(x.EndpointName)).ToList();

            // TODO: reformat this line of code / it's really ugly
            // build a string describing all endpoint status changes
            updateMsg.EndpointStatus = (updateMsg.EndpointsThatJustDied.Any() ? updateMsg.EndpointsThatJustDied.Count().ToString()
                + String.Format(" Endpoint{0} Died; ", updateMsg.EndpointsThatJustDied.Count > 1 ? "s" : "") : String.Empty)
                + (updateMsg.EndpointsThatJustBecameAlive.Any() ? updateMsg.EndpointsThatJustBecameAlive.Count().ToString() 
                    + (String.Format(" Endpoint{0} Alive Again; ", updateMsg.EndpointsThatJustBecameAlive.Count > 1 ? "s" : "")) : String.Empty);

            // find checks that show in the list of current failed checks in the message, but that weren't already in the saga data's list of failed checks
            updateMsg.CustomChecksThatJustStartedFailing = sagaData.FailedCustomChecks.Where(x => sagaData.PreviouslyFailedCustomChecks == null
                || !sagaData.PreviouslyFailedCustomChecks.Select(y => y.CustomCheckId).Contains(x.CustomCheckId)).ToList();

            // find checks that show in saga data state as failed, but that weren't included in the list of current failed checks in the message
            updateMsg.CustomChecksThatJustStoppedFailing = sagaData.RestoredCustomChecks.Where(x => sagaData.PreviouslyRestoredCustomChecks == null
                || !sagaData.PreviouslyRestoredCustomChecks.Select(y => y.CustomCheckId).Contains(x.CustomCheckId)).ToList();

            // build a string describing all custom check status changes
            updateMsg.CheckStatus = (updateMsg.CustomChecksThatJustStartedFailing.Any() ? updateMsg.CustomChecksThatJustStartedFailing.Count().ToString() 
                    + (String.Format(" Check{0} Started Failing; ", updateMsg.CustomChecksThatJustStartedFailing.Count() > 1 ? "s" : "")) : String.Empty)
                + (updateMsg.CustomChecksThatJustStoppedFailing.Any() ? updateMsg.CustomChecksThatJustStoppedFailing.Count().ToString() 
                    + (String.Format(" Check{0} Passing Again; ", updateMsg.CustomChecksThatJustStoppedFailing.Count > 1 ? "s" : "")) : String.Empty);

            updateMsg.DeadEndpointCount = message.DeadEndpointCount;
            updateMsg.FailedCheckCount = message.FailedCheckCount;
            updateMsg.ServicePulseUrl = new ServiceControlMonitorSettings().ServicePulseUrl;

            // did anything change?
            if ((updateMsg.EndpointStatus + updateMsg.CheckStatus + updateMsg.FailedMsgStatus) != String.Empty)
            {
                string subject = String.Format("ESB Monitor: {0}{1}{2}", updateMsg.EndpointStatus, updateMsg.CheckStatus, updateMsg.FailedMsgStatus);
                subject = subject.TrimEnd(new char[] { ';', ' ' });

                updateMsg.ChangeSummary = subject;

                return updateMsg;
            }
            else
            {
                return null;
            }
        }
    }
}
