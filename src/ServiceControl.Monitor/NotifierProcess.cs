namespace EaiGuy.ServiceControl.Monitor
{
    using Email;
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NotifierProcess : IWantToRunWhenBusStartsAndStops
    {
        private ServiceControlMonitorSettings settings { get; set; }
        private static readonly ILog log = LogManager.GetLogger(typeof(NotifierProcess));
        private IBus bus { get; set; }
        private Schedule schedule { get; set; }

        public NotifierProcess(IBus bus, Schedule schedule)
        {
            this.settings = new ServiceControlMonitorSettings();
            this.bus = bus;
            this.schedule = schedule;
        }

        public void Start()
        {
            try
            {
                int interval = 15;
                var emailSettings = new EmailSettings();

                if (!String.IsNullOrWhiteSpace(emailSettings.WriteACopyOfAllEmailsToThisFolder))
                {
                    DirectoryHelpers.ValidatePath(emailSettings.WriteACopyOfAllEmailsToThisFolder, "WriteACopyOfAllEmailsToThisFolder");
                }

                // Schedule the task
                if (settings.BatchAndPublishMinutes.HasValue)
                {
                    interval = settings.BatchAndPublishMinutes.Value;
                    log.InfoFormat("Configuring the NotifierProcess to run every {0} minute(s) starting now.", interval);
                }
                else
                {
                    log.WarnFormat("The setting 'BatchAndPublishMinutes' was not found, so configuring the NotifierProcess to run every {0} minutes starting now.", settings.BatchAndPublishMinutes);
                }

                this.schedule.Every(TimeSpan.FromMinutes(interval),
                    () => new Notifier(this.bus).Run());

                // go ahead and run the notifier
                new Notifier(this.bus).Run();
            }
            catch (Exception e)
            {
                log.Fatal("The NotifierProcess's Start method encountered a fatal error. Error: " + e.ToString());

                throw;
            }
        }

        public void Stop()
        {
        }
    }
}
