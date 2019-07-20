using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Data.Sources;
using HelsiApi.Configuration;
using HelsiApi.Midleware.ExceptionHandler;
using HelsiApi.Midleware.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelsiApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SQL")));

            services.AddMvc(options => { options.Filters.Add<ValidateModelStateAttribute>(); }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerDocument();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<AutofacDiModule>();
            containerBuilder.RegisterModule (new ElacsticCLientModule(Configuration));
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCustomExceptionsMiddleware();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
