using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HelloSignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // RunAsService
            // dotnet publish --configuration Release --self-contained -r win10-x64 --output d:\svc
            // sc create HelloSignalR binPath= "d:\svc\HelloSignalR.exe"
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());

            var host = builder.Build();

            if (isService)
            {
                //host.RunAsService();
                host.RunAsCustomService();
            }
            else
            {
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseUrls("http://localhost:5100", "https://localhost:5101") // Unable to configure HTTPS endpoint
                .UseUrls("http://localhost:5100")
                .UseStartup<Startup>();
    }
}
