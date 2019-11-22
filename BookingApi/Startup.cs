/* Copyright (c) Mark Seemann 2019 all rights reserved
 * Permission is hereby granted to share this code for educational purposes
 * only, under the condition that this header remains intact. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ploeh.Samples.BookingApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var seatingDuration =
                Configuration.GetValue<TimeSpan>("SeatingDuration");
            var tables = Configuration.GetSection("Tables")
                .Get<int[]>()
                .Select(i => new Table(i))
                .ToArray();
            var capacity = Configuration.GetValue<int>("Capacity");
            var connectionString = Configuration.GetConnectionString("Booking");
            services.AddSingleton<IControllerActivator>(
                new CompositionRoot(seatingDuration, tables, connectionString));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
