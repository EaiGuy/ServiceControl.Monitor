namespace EaiGuy.ServiceControl.Monitor
{
    using System;
    public static class SchedulerHelpers
    {
        public static bool IsQuietTime(ServiceControlMonitorSettings settings)
        {
            bool todayIsQuietDay = settings.QuietTimeDays.ToLower().Contains(DateTime.Now.DayOfWeek.ToString().ToLower());

            if (String.IsNullOrWhiteSpace(settings.QuietTimeBeginTime) || settings.QuietTimeLengthHours == 0)
            {
                // no begin time or hours specified, so only return true if today is a quiet day
                return todayIsQuietDay;
            }

            var quietTimeBeginTime = DateTime.SpecifyKind(DateTime.Parse(settings.QuietTimeBeginTime), DateTimeKind.Local);
            bool inYesterdaysQuietTime = quietTimeBeginTime.AddDays(-1).AddHours(settings.QuietTimeLengthHours) >= DateTime.Now;
            bool inTodaysQuietTime = quietTimeBeginTime <= DateTime.Now && quietTimeBeginTime.AddHours(settings.QuietTimeLengthHours) >= DateTime.Now;

            return todayIsQuietDay || inYesterdaysQuietTime || inTodaysQuietTime;
        }
    }
}
