//namespace ServiceControl.CompositeViews.Endpoints
namespace EaiGuy.ServiceControl.Monitor.Messages.ServiceControl
{
    using System;

    // Borrowed this class from ServiceControl source code on GitHub
    public class HeartbeatInformation
    {
        public DateTime LastReportAt { get; set; }
        public Status ReportedStatus { get; set; }
    }
}