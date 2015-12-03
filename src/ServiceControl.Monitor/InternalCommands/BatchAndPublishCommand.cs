namespace EaiGuy.ServiceControl.Monitor.InternalCommands
{
    using System;
    using Messages.ServiceControl;
    using Messages.ServiceControl.CustomChecks;

    public class BatchAndPublishCommand
    {
        public int One { get { return 1; } }
        public int DeadEndpointCount { get; set; }
        public int FailedCheckCount { get; set; }
    }
}
