using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HelloNLog
{
    public class LoggingFilterAttribute : IActionFilter
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // do something before the action executes
            try
            {
                if (context.ActionArguments.Count > 0)
                    _logger.Trace(JsonConvert.SerializeObject(context.ActionArguments.ToDictionary(d => d.Key, d => d.Value)));
                else
                    _logger.Trace(context.HttpContext.Request.Method);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}