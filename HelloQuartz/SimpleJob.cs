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
        public virtual Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string name = dataMap.GetString("name");
            Console.WriteLine($"{DateTime.Now} Execute {name}");
            return Task.FromResult(true);
        }
    }
}