using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerService.Consumer
{
    public class Message
    {
        public Message()
        {
            Id = Guid.NewGuid();
            
            CreatedAt = DateTime.Now;
        }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
