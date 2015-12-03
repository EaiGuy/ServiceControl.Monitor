//namespace ServiceControl.CustomChecks
namespace EaiGuy.ServiceControl.Monitor.Messages.ServiceControl.CustomChecks
{
    using System;
    using System.Collections.Generic;

    // Borrowed this class from ServiceControl source code on GitHub
    public class CustomCheck
    {
        public Guid Id { get; set; }
        public string CustomCheckId { get; set; }
        public string Category { get; set; }
        public Status Status { get; set; }
        public DateTime ReportedAt { get; set; }
        public string FailureReason { get; set; }
        public EndpointDetails OriginatingEndpoint { get; set; }
    }

    public class CustomCheckComparer : IEqualityComparer<CustomCheck>
    {
        public bool Equals(CustomCheck x, CustomCheck y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(CustomCheck obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}