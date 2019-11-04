using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Consumer
{
    public class MessageHandler : IHandleMessages<Message>
    {
        //private readonly ILogger _logger;
        //public MessageHandler(ILogger logger)
        //{
        //    _logger = logger;
        //}
        public MessageHandler()
        {
            Console.WriteLine("eee");
        }
        public Task Handle(Message message)
        {
            Console.WriteLine($"Handling message ${message.Id} created at ${message.CreatedAt}");
            return Task.CompletedTask;
            //_logger.LogInformation($"Handling message ${message.Id} created at ${message.CreatedAt}");
            //throw new NotImplementedException();
        }
    }
}
