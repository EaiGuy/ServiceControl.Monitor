namespace EaiGuy.ServiceControl.Monitor.Messages.Events
{
    using System;
    using System.Collections.Generic;
    using global::ServiceControl.Contracts;

    public interface IHealthStatusChanged
    {
        string ChangeSummary { get; set; }
        string EndpointStatus { get; set; }
        string CheckStatus { get; set; }
        int DeadEndpointCount { get; set; }
        int FailedCheckCount { get; set; }
        string FailedMsgStatus { get; set; }
        List<HeartbeatStopped> EndpointsThatJustDied { get; set; }
        List<HeartbeatRestored> EndpointsThatJustBecameAlive { get; set; }
        List<CustomCheckFailed> CustomChecksThatJustStartedFailing { get; set; }
        List<CustomCheckSucceeded> CustomChecksThatJustStoppedFailing { get; set; }
        List<MessageFailed> FailedMessages { get; set; }
        string ServicePulseUrl { get; set; }
    }
}
