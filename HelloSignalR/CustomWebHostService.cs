using System;
using System.ServiceProcess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using NLog;

namespace HelloSignalR
{
    internal class CustomWebHostService : WebHostService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public CustomWebHostService(IWebHost host) : base(host)
        {
        }

        protected override void OnStarting(string[] args)
        {
            // Log
            _logger.Info("OnStarting");
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            // More log
            _logger.Info("OnStarted");
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            // Even more log
            _logger.Info("OnStopping");
            base.OnStopping();
        }
    }

    public static class CustomWebHostWindowsServiceExtensions
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        public static void RunAsCustomService(this IWebHost host)
        {
            try
            {
                var webHostService = new CustomWebHostService(host);
                ServiceBase.Run(webHostService);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}