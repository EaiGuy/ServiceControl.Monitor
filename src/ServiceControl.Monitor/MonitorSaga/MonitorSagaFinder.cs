namespace EaiGuy.ServiceControl.Monitor.MonitorSaga
{
    using global::ServiceControl.Contracts;
    using NServiceBus.RavenDB.Persistence;
    using NServiceBus.Saga;
    using System.Linq;
    using System;

    class MonitorSagaFinder : IFindSagas<MonitorSagaData>.Using<MessageFailed>,
        IFindSagas<MonitorSagaData>.Using<HeartbeatStopped>,
        IFindSagas<MonitorSagaData>.Using<HeartbeatRestored>,
        IFindSagas<MonitorSagaData>.Using<CustomCheckFailed>,
        IFindSagas<MonitorSagaData>.Using<CustomCheckSucceeded>
    {
        public ISessionProvider SessionProvider { get; set; }

        // there should only be one saga instance total

        public MonitorSagaData FindBy(CustomCheckFailed message)
        {
            return this.SessionProvider.Session.Query<MonitorSagaData>()
                .SingleOrDefault(x => x.One == 1);
        }

        public MonitorSagaData FindBy(CustomCheckSucceeded message)
        {
            return this.SessionProvider.Session.Query<MonitorSagaData>()
                .SingleOrDefault(x => x.One == 1);
        }

        public MonitorSagaData FindBy(HeartbeatRestored message)
        {
            return this.SessionProvider.Session.Query<MonitorSagaData>()
                .SingleOrDefault(x => x.One == 1);
        }

        public MonitorSagaData FindBy(HeartbeatStopped message)
        {
            return this.SessionProvider.Session.Query<MonitorSagaData>()
                .SingleOrDefault(x => x.One == 1);
        }

        public MonitorSagaData FindBy(MessageFailed message)
        {
            return this.SessionProvider.Session.Query<MonitorSagaData>()
                .SingleOrDefault(x => x.One == 1);
        }
    }
}
