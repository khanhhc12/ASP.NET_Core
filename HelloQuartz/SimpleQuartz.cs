using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;

namespace HelloQuartz
{
    public class SimpleQuartz
    {
        // First we must get a reference to a scheduler
        ISchedulerFactory sf = new StdSchedulerFactory();
        string dirJson = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Json");

        public async Task Run()
        {
            IScheduler sched = await sf.GetScheduler();

            string fileJson = Path.Combine(dirJson, "jobs.json");
            List<JobModel> list = new List<JobModel>();
            try
            {
                if (File.Exists(fileJson))
                    list = JsonConvert.DeserializeObject<List<JobModel>>(File.ReadAllText(fileJson));
            }
            catch { }
            if (list == null || list.Count == 0) return;

            foreach (var item in list)
            {
                JobKey jobKey = new JobKey($"job{item.Name}", $"group{item.Name}");
                if (item.IsEnabled)
                {
                    if (await sched.CheckExists(jobKey)) return;

                    if (item.IntervalSchedule == null) return;
                    if (item.IntervalSchedule.Months == 0 && item.IntervalSchedule.Days == 0
                        && item.IntervalSchedule.Hours == 0 && item.IntervalSchedule.Minutes == 0) return;

                    // computer a time that is on the next round minute
                    DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.Now);
                    if (item.StartAt != null)
                        runTime = item.StartAt.GetDateTimeOffset();

                    Console.WriteLine($"{runTime.DateTime} DateTimeOffset {item.Name}");

                    // define the job and tie it to our SimpleJob class
                    IJobDetail job = JobBuilder.Create<SimpleJob>()
                        .WithIdentity($"job{item.Name}", $"group{item.Name}")
                        .UsingJobData("name", item.Name)
                        .Build();

                    // Trigger the job to run on the next round minute
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity($"trigger{item.Name}", $"group{item.Name}")
                        .StartAt(runTime)
                        .WithCalendarIntervalSchedule(x =>
                        {
                            if (item.IntervalSchedule.Months > 0)
                                x.WithIntervalInMonths(item.IntervalSchedule.Months);
                            else if (item.IntervalSchedule.Days > 0)
                                x.WithIntervalInDays(item.IntervalSchedule.Days);
                            else if (item.IntervalSchedule.Hours > 0)
                                x.WithIntervalInHours(item.IntervalSchedule.Hours);
                            else if (item.IntervalSchedule.Minutes > 0)
                                x.WithIntervalInMinutes(item.IntervalSchedule.Minutes);
                        })
                        .Build();

                    // Tell quartz to schedule the job using our trigger
                    if (!await sched.CheckExists(job.Key))
                        await sched.ScheduleJob(job, trigger);
                }
                else
                {
                    if (await sched.CheckExists(jobKey))
                        await sched.DeleteJob(jobKey);
                }
            }

            // Start up the scheduler (nothing can actually run until the scheduler has been started)
            await sched.Start();
        }
    }
}