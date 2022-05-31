using InventoryService.DBContext;
using InventoryService.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContextPool<InventoryDbContext>
            (options => options.UseSqlServer(Configuration.GetConnectionString("FlightDatabase")));
            services.AddTransient<IInventoryRepository, InventoryRepository>();
            services.AddSingleton(sp => RabbitHutch.CreateBus("localhost"));

            //var factory = new ConnectionFactory
            //{
            //    Uri = new Uri("amqp://guest:guest@localhost:5672/")
            //};
            //var connection = factory.CreateConnection();
            //var channel = connection.CreateModel();
            //InventoryRepository.Publish(channel);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvcWithDefaultRoute();
            app.UseMvc();

          
        }
    }
}
