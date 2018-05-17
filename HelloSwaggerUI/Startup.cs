using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using HelloSwaggerUI.Models;

namespace HelloSwaggerUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // AddSwaggerGen
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "My API", Version = "v1", Description = "Description" });
                // TagActionsBy
                //c.TagActionsBy(api => api.HttpMethod);
                // OrderActionsBy
                //c.OrderActionsBy(api => api.RelativePath);
                var dirPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "xml");
                if (Directory.Exists(dirPath))
                {
                    var dir = new DirectoryInfo(dirPath);
                    var files = dir.GetFiles("*.xml");
                    foreach (FileInfo file in files)
                        c.IncludeXmlComments(file.FullName);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            //UseSwaggerAuthorized
            app.UseSwaggerAuthorized();
            //UseSwagger
            app.UseSwagger();
            //UseSwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
