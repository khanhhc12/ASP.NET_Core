using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace HelloQuartz
{
    public class SimpleQuartz
    {
        public async Task Run()
        {
            // First we must get a reference to a scheduler
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = await sf.GetScheduler();

            // computer a time that is on the next round minute
            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<SimpleJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run on the next round minute
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                //.StartAt(runTime)
                //.WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            await sched.ScheduleJob(job, trigger);

            // Start up the scheduler (nothing can actually run until the
            // scheduler has been started)
            await sched.Start();
        }
    }
}