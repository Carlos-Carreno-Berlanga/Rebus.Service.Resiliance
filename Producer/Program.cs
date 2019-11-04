using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddEnvironmentVariables();

            if (args != null)
            {
                config.AddCommandLine(args);
            }
        })
        .ConfigureServices((hostContext, services) =>
        {
            var provider = services.BuildServiceProvider();

            var loggerFactory = new LoggerFactory();
            var serviceProvider = services.BuildServiceProvider();
            _logger = serviceProvider.GetService<ILogger<Program>>();
            services.AddOptions();
            services.Configure<DaemonConfig>(hostContext.Configuration.GetSection("Daemon"));
            services.AddScoped<IEnviromentVariableService, EnviromentVariableService>();
            services.AddScoped<IMeasurementService, MeasurementService>();
            services.AddScoped<IMeasurementRepository, MeasurementRepository>();
            services.AddSingleton<IHostedService, DaemonService>();

            services.AddRebus(configure => configure
        .Logging(l => l.Use(new MSLoggerFactoryAdapter(loggerFactory)))
        .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://pklfurgc:4YJosxjltR4AntkkvVignFH-TKW16c9k@raven.rmq.cloudamqp.com/pklfurgc")
        .InputQueueOptions((cfg => { cfg.SetDurable(true); cfg.SetAutoDelete(false, 600000); }))

        )
        .Routing(r => r.TypeBased().Map<MeterMessage>("messages-queueV2"))
        );

        })
        .ConfigureLogging((hostingContext, logging) =>
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
        });
            //_logger.LogInformation("TEST");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            await builder.RunConsoleAsync();
        }
    }
}
