//namespace ServiceControl.Contracts.Operations
namespace EaiGuy.ServiceControl.Monitor.Messages.ServiceControl
{
    using System;

    // Borrowed this class from ServiceControl source code on GitHub
    public class EndpointDetails
    {
        public string Name { get; set; }

        public Guid HostId { get; set; }

        public string Host { get; set; }
    }
}