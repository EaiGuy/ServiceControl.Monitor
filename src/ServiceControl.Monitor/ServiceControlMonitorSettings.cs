namespace EaiGuy.ServiceControl.Monitor
{
    using System;
    using System.Configuration;

    public class ServiceControlMonitorSettings
    {
        /// <summary>
        /// Specify the ServiceControl URI that this service will use to count the total number of failed custom checks and dead endpoints.
        /// </summary>
        internal string ServiceControlApiUri { get { return ConfigurationManager.AppSettings["ServiceControlApiUri"]; } }

        /// <summary>
        /// Configure the address to which this service should send a notificaiton email if ServiceControl is ever unavailable.
        /// </summary>
        internal string ErrorEmailAddress { get { return ConfigurationManager.AppSettings["ErrorEmailAddress"]; } }

        /// <summary>
        /// Use this setting to specify the interval for aggregating notifications and publishing IHealthStatusChanged events
        /// </summary>
        internal int? BatchAndPublishMinutes { get { return Convert.ToInt16(ConfigurationManager.AppSettings["BatchAndPublishMinutes"]); } }

        /// <summary>
        /// Configure the start time of night when IHealthStatusChanged events should not be published. 
        /// </summary>
        internal string QuietTimeBeginTime { get { return ConfigurationManager.AppSettings["QuietTimeBeginTime"]; } }

        /// <summary>
        /// Configure how many hours after QuietTimeBeginTime IHealthStatusChanged should not be published. When these hours are up, 
        /// this service will publish any health status changes or failed messages that happened during these quiet hours.
        /// </summary>
        internal int QuietTimeLengthHours { get { return Convert.ToInt16(ConfigurationManager.AppSettings["QuietTimeLengthHours"]); } }

        /// <summary>
        /// Configure days during which IHealthStatusChanged events will not be published. When these days are passed, this service
        /// will publish any health status changes or failed messages that happened during these days.
        /// </summary>
        internal string QuietTimeDays { get { return ConfigurationManager.AppSettings["QuietTimeDays"]; } }

        /// <summary>
        /// This url is published in the IHealthStatusChanged event so notifiers can provide users with the link to ServicePulse.
        /// </summary>
        public string ServicePulseUrl { get { return ConfigurationManager.AppSettings["ServicePulseUrl"]; } }

        /// <summary>
        /// Use this setting to exclude exception stack-trace details when publishing the IHealthStatusChanged event to reduce the 
        /// risk of exceeding MSMQ's 4MB limit. Stack-trace details for a single failed message are typically over 11KB.
        /// </summary>
        public bool IncludeStackTraceInEvent { get { return string.IsNullOrEmpty(ConfigurationManager.AppSettings["IncludeStackTraceInEvent"]) ? false
                    : bool.Parse(ConfigurationManager.AppSettings["IncludeStackTraceInEvent"]); } }

        /// <summary>
        /// Use this setting to cap the number of failed messages published in the IHealthStatusChanged event to prevent
        /// the message from exceeding MSMQ's 4MB limit. If not specified, this setting defaults to 50.
        /// </summary>
        public int MaxFailedMessagesToIncludeInEvent { get { return string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxFailedMessagesToIncludeInEvent"]) ? 50
                    : int.Parse(ConfigurationManager.AppSettings["MaxFailedMessagesToIncludeInEvent"]); } }
    }
}
