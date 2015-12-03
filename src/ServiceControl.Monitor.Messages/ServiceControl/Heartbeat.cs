//namespace ServiceControl.HeartbeatMonitoring
namespace EaiGuy.ServiceControl.Monitor.Messages.ServiceControl
{
    // Borrowed this class from ServiceControl source code on GitHub
    using System;
    //using Contracts.Operations;

    //public class Heartbeat
    //{
    //    public Guid Id { get; set; }
    //    public DateTime LastReportAt { get; set; }
    //    public EndpointDetails EndpointDetails { get; set; }
    //    public Status ReportedStatus { get; set; }
    //    public bool Disabled { get; set; }
    //}

    public enum Status
    {
        Beating,
        Dead
    }
}