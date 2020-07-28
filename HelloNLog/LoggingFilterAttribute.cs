using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using Newtonsoft.Json;
using System;
using System.Linq;

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
                string method = (context.HttpContext.Request.Method ?? "").ToUpper();
                _logger.Trace(method == "GET" ? "GET" : JsonConvert.SerializeObject(context.ActionArguments));
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
