using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using System;

namespace Web.Consumer
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

 
            services.AutoRegisterHandlersFromAssemblyOf<MessageHandler>();
            services.AddRebus(configure => configure
            //.Logging(l => l.Use(new MSLoggerFactoryAdapter(loggerFactory)))
            .Transport(t => t.UseRabbitMq(Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION"), "messages-queue"))
            .Routing(r => r.TypeBased().MapAssemblyOf<Message>("messages-queue")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            
            app.UseRebus();

        }
    }
}
