using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Consumer
{
    public class MessageHandler : IHandleMessages<Message>
    {
        private readonly ILogger<MessageHandler> _logger;
        private Random _random = new Random();
        public MessageHandler(ILogger<MessageHandler> logger)
        {
            _logger = logger;
        }


        public Task Handle(Message message)
        {
            _logger.LogWarning($"Handling message ${message.Id} created at ${message.CreatedAt}");
            if (_random.Next(10) % 5 == 0)
            {
                throw new Exception($"message ${message.Id} failed");
            }
            _logger.LogWarning($"Message ${message.Id} handled succesfully");
            return Task.CompletedTask;
        }
    }
}
