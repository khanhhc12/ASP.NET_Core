using System;
using System.IO;
using HelloSwaggerUI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddControllers();

            // AddSwaggerGen
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1", Description = "Description" });
                // TagActionsBy
                //c.TagActionsBy(api => api.HttpMethod);
                // OrderActionsBy
                //c.OrderActionsBy(api => api.RelativePath);
                var dirPath = Path.Combine(AppContext.BaseDirectory, "xml");
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

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
