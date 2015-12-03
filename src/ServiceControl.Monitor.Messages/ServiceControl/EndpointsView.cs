//namespace ServiceControl.CompositeViews.Endpoints
namespace EaiGuy.ServiceControl.Monitor.Messages.ServiceControl
{
    using System;
    using System.Collections.Generic;

    // Borrowed this class from ServiceControl source code on GitHub
    public class EndpointsView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string HostDisplayName { get; set; }

        public bool Monitored { get; set; }

        public bool MonitorHeartbeat { get; set; }
        public string LicenseStatus { get; set; }
        public HeartbeatInformation HeartbeatInformation { get; set; }
        public bool IsSendingHeartbeats { get; set; }
   }

    public class EndpointsViewComparer : IEqualityComparer<EndpointsView>
    {
        public bool Equals(EndpointsView x, EndpointsView y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(EndpointsView obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}