namespace EaiGuy.ServiceControl.Monitor.MonitorSaga
{
    using global::ServiceControl.Contracts;
    using InternalCommands;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.Saga;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MonitorSaga : Saga<MonitorSagaData>, 
        IAmStartedByMessages<BatchAndPublishCommand>, 
        IHandleMessages<MessageFailed>,
        IHandleMessages<CustomCheckFailed>,
        IHandleMessages<CustomCheckSucceeded>,
        IHandleMessages<HeartbeatStopped>,
        IHandleMessages<HeartbeatRestored>
    {
        internal static ServiceControlMonitorSettings Settings { get { return new ServiceControlMonitorSettings(); } }
        private static readonly ILog log = LogManager.GetLogger(typeof(MonitorSaga));
        public IMessageCreator MessageCreator { get; set; }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MonitorSagaData> mapper)
        {
            mapper.ConfigureMapping<BatchAndPublishCommand>(m => m.One).ToSaga(s => s.One);
            // see MonitorSagaFinder.cs for mappings for other message types
        }

        public void Handle(BatchAndPublishCommand message)
        {
            var scUpdatedEvent = EventGenerator.GenerateEventMsg(message, this.Data, this.MessageCreator);
            if (scUpdatedEvent == null)
            {
                log.Debug("No endpoint or heartbeat changes or failed messages found. Aborting.");
                return;
            }
            else
            {
                // publish results
                this.Bus.Publish(scUpdatedEvent);
            }
            
            // update saga data with current status
            this.Data.FailedMessages = new List<MessageFailed>();
            this.Data.PreviouslyDeadEndpoints = scUpdatedEvent.EndpointsThatJustDied;
            this.Data.PreviouslyRestoredEndpoints = scUpdatedEvent.EndpointsThatJustBecameAlive;
            this.Data.PreviouslyFailedCustomChecks = scUpdatedEvent.CustomChecksThatJustStartedFailing;
            this.Data.PreviouslyRestoredCustomChecks = scUpdatedEvent.CustomChecksThatJustStoppedFailing;
            this.Data.FailedCustomChecks = new List<CustomCheckFailed>();
            this.Data.RestoredCustomChecks = new List<CustomCheckSucceeded>();
            this.Data.DeadEndpoints = new List<HeartbeatStopped>();
            this.Data.RestoredEndpoints = new List<HeartbeatRestored>();
        }

        public void Handle(MessageFailed message)
        {
            log.InfoFormat("Received a MessageFailed message of type: ", message.MessageType);

            if (!Settings.IncludeStackTraceInEvent)
            {
                try
                {
                    message.FailureDetails.Exception.StackTrace = String.Empty;
                    message.MessageDetails.Headers["NServiceBus.ExceptionInfo.StackTrace"] = String.Empty;
                }
                catch (Exception e)
                {
                    log.Warn("Failed to delete stack trace from failed message. Details: " + e.Message);
                }
            }

            this.Data.FailedMessages.Add(message);
        }

        public void Handle(CustomCheckFailed message)
        {
            // remove from the Restored list if present
            var restoredCheck = this.Data.RestoredCustomChecks.SingleOrDefault(x => x.CustomCheckId == message.CustomCheckId);
            this.Data.RestoredCustomChecks.Remove(restoredCheck);

            // add to the failed collection
            this.Data.FailedCustomChecks.Add(message);
        }

        public void Handle(CustomCheckSucceeded message)
        {
            // remove from the failed list if present
            var failedCheck = this.Data.FailedCustomChecks.SingleOrDefault(x => x.CustomCheckId == message.CustomCheckId);
            this.Data.FailedCustomChecks.Remove(failedCheck);

            // add to the restored collection
            this.Data.RestoredCustomChecks.Add(message);
        }

        public void Handle(HeartbeatStopped message)
        {
            // remove from the restored list if present
            var restoredEndpoint = this.Data.RestoredEndpoints.SingleOrDefault(x => 
                x.HostId == message.HostId && x.EndpointName == message.EndpointName);
            this.Data.RestoredEndpoints.Remove(restoredEndpoint);

            // add to the failed collection
            this.Data.DeadEndpoints.Add(message);
        }

        public void Handle(HeartbeatRestored message)
        {
            // remove from the failed list if present
            var deadEndpoint = this.Data.DeadEndpoints.SingleOrDefault(x =>
                x.HostId == message.HostId && x.EndpointName == message.EndpointName);
            this.Data.DeadEndpoints.Remove(deadEndpoint);

            // add to the restored collection
            this.Data.RestoredEndpoints.Add(message);
        }
    }
}
