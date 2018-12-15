using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace HelloQuartz
{
    public class SimpleJobEx : IJob
    {        
        public virtual Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string name = dataMap.GetString("name");
            Console.WriteLine($"{DateTime.Now} Execute {name}");
            return Task.FromResult(true);
        }
    }
}