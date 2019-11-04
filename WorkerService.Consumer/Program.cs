using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

namespace WorkerService.Consumer
{
    public class Program
    {

        public Program()
        {

        }

        public static void Main(string[] args)
        {

            //using (var activator = new BuiltinHandlerActivator())
            //{
            //    var loggerFactory = new LoggerFactory();

            //activator.Register(() => new MessageHandler());
            //var bus = Configure.With(activator)
            //    .Logging(l => l.Use(new MSLoggerFactoryAdapter(loggerFactory)))
            //    .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://pklfurgc:4YJosxjltR4AntkkvVignFH-TKW16c9k@raven.rmq.cloudamqp.com/pklfurgc"))
            //    .Routing(r => r.TypeBased().MapAssemblyOf<Message>("messages-queue"))
            //.Start();
            //}

            var services = new ServiceCollection();
            services.AutoRegisterHandlersFromAssemblyOf<Message>();

            var loggerFactory = new LoggerFactory();
            // 1.1. Configure Rebus
            services.AutoRegisterHandlersFromAssemblyOf<MessageHandler>();
            services.AddRebus(configure => configure
            .Logging(l => l.Use(new MSLoggerFactoryAdapter(loggerFactory)))
            .Transport(t => t.UseRabbitMq("amqp://pklfurgc:4YJosxjltR4AntkkvVignFH-TKW16c9k@raven.rmq.cloudamqp.com/pklfurgc", "messages-queue"))
            .Routing(r => r.TypeBased().MapAssemblyOf<Message>("messages-queue")));

            // 1.2. Potentially add more service registrations for the application, some of which
            //      could be required by handlers.

            // 2. Application starting pipeline...
            // Make sure we correctly dispose of the provider (and therefore the bus) on application shutdown
            using (var provider = services.BuildServiceProvider())
            {
                // 3. Application started pipeline...

                // 3.1. Now application is running, lets trigger the 'start' of Rebus.
                provider.UseRebus();
                //optionally...
                //provider.UseRebus(async bus => await bus.Subscribe<Message1>());

                // 3.2. Begin the domain work for the application
            }


                CreateHostBuilder(args).Build().Run();





        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //var loggerFactory = new LoggerFactory();



                    //services.AutoRegisterHandlersFromAssemblyOf<MessageHandler>();
                    //services.AddRebus(configure => configure
                    //.Logging(l => l.Use(new MSLoggerFactoryAdapter(loggerFactory)))
                    //.Transport(t => t.UseRabbitMq("amqp://pklfurgc:4YJosxjltR4AntkkvVignFH-TKW16c9k@raven.rmq.cloudamqp.com/pklfurgc", "messages-queue"))
                    //.Routing(r => r.TypeBased().MapAssemblyOf<Message>("messages-queue")));


                    services.AddHostedService<Worker>();

                    //using (var provider = services.BuildServiceProvider())
                    //{
                    //    // 3. Application started pipeline...

                    //    // 3.1. Now application is running, lets trigger the 'start' of Rebus.
                    //    //provider.UseRebus();
                    //    //optionally...
                    //    provider.UseRebus(async bus => await bus.Subscribe<Message>());

                    //    // 3.2. Begin the domain work for the application
                    //    //var producer = provider.GetRequiredService<Producer>();
                    //    //producer.Produce();
                    //}

                });



    }
}
