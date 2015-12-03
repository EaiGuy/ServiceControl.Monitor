namespace EaiGuy.ServiceControl.Monitor.MonitorSaga
{
    using global::ServiceControl.Contracts;
    using NServiceBus.Saga;
    using System.Collections.Generic;
    
    public class MonitorSagaData : ContainSagaData
    {
        [Unique]
        public int One { get { return 1; } }

        public List<HeartbeatStopped> DeadEndpoints { get; set; }
        public List<HeartbeatRestored> RestoredEndpoints { get; set; }
        public List<CustomCheckFailed> FailedCustomChecks { get; set; }
        public List<CustomCheckSucceeded> RestoredCustomChecks { get; set; }
        public List<HeartbeatStopped> PreviouslyDeadEndpoints { get; set; }
        public List<HeartbeatRestored> PreviouslyRestoredEndpoints { get; set; }
        public List<CustomCheckFailed> PreviouslyFailedCustomChecks { get; set; }
        public List<CustomCheckSucceeded> PreviouslyRestoredCustomChecks { get; set; }
        public List<MessageFailed> FailedMessages { get; set; }

        public MonitorSagaData()
        {
            this.DeadEndpoints = new List<HeartbeatStopped>();
            this.RestoredEndpoints = new List<HeartbeatRestored>();
            this.FailedCustomChecks = new List<CustomCheckFailed>();
            this.RestoredCustomChecks = new List<CustomCheckSucceeded>();
            this.PreviouslyDeadEndpoints = new List<HeartbeatStopped>();
            this.PreviouslyRestoredEndpoints = new List<HeartbeatRestored>();
            this.PreviouslyFailedCustomChecks = new List<CustomCheckFailed>();
            this.PreviouslyRestoredCustomChecks = new List<CustomCheckSucceeded>();
            this.FailedMessages = new List<MessageFailed>();
        }
    }
}
