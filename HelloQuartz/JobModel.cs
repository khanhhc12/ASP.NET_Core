using System;
using Quartz;

namespace HelloQuartz
{
    public class JobModel
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public StartAtModel StartAt { get; set; }
        public IntervalScheduleModel IntervalSchedule { get; set; }
    }

    public class StartAtModel
    {
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public DateTimeOffset GetDateTimeOffset()
        {
            DateTime now = DateTime.Now;
            int hour = Hour > 0 ? Hour : now.Hour;
            int minute = Minute > 0 ? Minute : now.Minute;
            int day = Day > 0 ? Day : now.Day;
            int month = Month > 0 ? Month : now.Month;
            int year = now.Year;
            DateTimeOffset runTime = DateBuilder.DateOf(hour, minute, 0, day, month, year);
            // month
            if (runTime.DateTime < now && Month > 0 && Month < now.Month)
                runTime = runTime.AddYears(1);
            // day
            if (runTime.DateTime < now && Day > 0 && Day < now.Day)
            {
                if (Month == 0)
                    runTime = runTime.AddMonths(1);
                else
                    runTime = runTime.AddYears(1);

            }
            // hour
            if (runTime.DateTime < now && Hour > 0 && Hour < now.Hour)
            {
                if (Day == 0)
                    runTime = runTime.AddDays(1);
                else if (Month == 0)
                    runTime = runTime.AddMonths(1);
                else
                    runTime = runTime.AddYears(1);
            }
            // minute
            if (runTime.DateTime < now && Minute > 0 && Minute < now.Minute)
            {
                if (Hour == 0)
                    runTime = runTime.AddHours(1);
                else if (Day == 0)
                    runTime = runTime.AddDays(1);
                else if (Month == 0)
                    runTime = runTime.AddMonths(1);
                else
                    runTime = runTime.AddYears(1);
            }
            return runTime;
        }
    }

    public class IntervalScheduleModel
    {
        public int Months { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
    }
}