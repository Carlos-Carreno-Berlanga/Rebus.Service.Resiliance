using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using System;
using Web.Consumer;

namespace WorkerService.Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.AddRebus(configure => configure
                    .Transport(t => t.UseRabbitMqAsOneWayClient(Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION"))
                    .InputQueueOptions((cfg => { cfg.SetDurable(true); cfg.SetAutoDelete(false, 600000); }))

                    )
                    .Routing(r => r.TypeBased().Map<Message>("messages-queue"))
                    );
                });
    }
}
