using MediatorPatternExample.Domain.Customer.Command;
using MediatorPatternExample.Domain.Customer.Handler;
using MediatorPatternExample.Infra;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MediatorPatternExample
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
            services.AddSingleton<ICustomerRepository, CustomerRepository>();

            //services.AddScooped<IRequestHandler<CustomerCreateCommand, string>>(c =>
            //{
            //    return new CustomerHandler(
            //        c.GetService<ICustomerRepository>(),
            //        c.GetService<ILogger<CustomerHandler>>(),
            //        true
            //        );
            //});

            //services.AddSingleton<IRequestHandler<CustomerCreateCommand, string>>(c =>
            //{
            //    return new CustomerHandler(
            //        c.GetService<ICustomerRepository>(),
            //        c.GetService<ILogger<CustomerHandler>>(),
            //        true
            //        );
            //});

            services.AddMediatR(typeof(Startup));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
