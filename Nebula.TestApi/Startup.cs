using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using Nebula.DataAccessLibrary.EntityFramework;
using Nebula.Membership;
using Nebula.Membership.Middlewares;
using Nebula.SeedLib;
using Nebula.TestApi.Middlewares;

namespace Nebula.TestApi
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
            ApplicationSettings.ConnectionString =
                "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=12345";
            ApiBootstrapper bootstrapper = new ApiBootstrapper();
            bootstrapper.Start();

            foreach (var service in bootstrapper.Container.ServiceCollection)
            {
                services.AddTransient(service.ServiceType, service.ImplementationType);
            }

            
           
            
            services.BuildServiceProvider();
            
            ITableCreator tableCreator = DependencyService.GetService<ITableCreator>();
            tableCreator.CreateAllTable<EntityBase>();

            var envService = services.FirstOrDefault(v => v.ServiceType == typeof(IHostingEnvironment));
            bootstrapper.Container.Register(envService.ServiceType,envService.ImplementationInstance);
            
//            ISeedManager seedManager = DependencyService.GetService<ISeedManager>();
//            seedManager.ExecuteAll();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<EntityUpdateMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action=list}/{id?}");
            });
        }
    }
}
