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
    public class SimpleJob : IJob
    {
        ISchedulerFactory sf = new StdSchedulerFactory();

        public virtual async Task Execute(IJobExecutionContext context)
        {
            //Console.WriteLine(DateTime.Now);
            await Run();
            //return Task.FromResult(true);
        }

        public async Task Run()
        {
            IScheduler sched = await sf.GetScheduler();

            // FileWatcher
            string dirJson = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Json");
            FileWatcher.CreateFileWatcher(dirJson);
            if (!FileWatcher.IsChanged) return;

            // log
            Console.WriteLine($"{DateTime.Now} Run");

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
                if (item.IsEnabled)
                {
                    if (item.IntervalSchedule == null) return;
                    if (item.IntervalSchedule.Months == 0 && item.IntervalSchedule.Days == 0
                        && item.IntervalSchedule.Hours == 0 && item.IntervalSchedule.Minutes == 0) return;

                    
                    DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.Now);
                    if (item.StartAt != null)
                        runTime = item.StartAt.GetDateTimeOffset();

                    Console.WriteLine($"{runTime.DateTime} DateTimeOffset {item.Name}");

                    IJobDetail job = JobBuilder.Create<SimpleJobEx>()
                        .WithIdentity($"job{item.Name}", $"group{item.Name}")
                        .UsingJobData("name", item.Name)
                        .Build();


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

                    if (!await sched.CheckExists(job.Key))
                        await sched.ScheduleJob(job, trigger);
                }
                else
                {
                    IJobDetail job = JobBuilder.Create<SimpleJobEx>()
                        .WithIdentity($"job{item.Name}", $"group{item.Name}")
                        .Build();

                    if (await sched.CheckExists(job.Key))
                        await sched.DeleteJob(job.Key);
                }
            }

            FileWatcher.IsChanged = false;

            await sched.Start();
        }
    }
}