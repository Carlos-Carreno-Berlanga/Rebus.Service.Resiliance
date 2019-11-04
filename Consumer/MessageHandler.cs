using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    public class MessageHandler : IHandleMessages<Message>
    {
        public Task Handle(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
