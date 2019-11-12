using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using System;

namespace Web.Consumer
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole()
                    .AddEventLog();
            });

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Debug));
            services.AutoRegisterHandlersFromAssemblyOf<MessageHandler>();
            services.AddRebus(configure => configure
            .Logging(l => l.Use(new MSLoggerFactoryAdapter(_loggerFactory)))
            .Options(o => o.SimpleRetryStrategy(maxDeliveryAttempts: 100))
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
