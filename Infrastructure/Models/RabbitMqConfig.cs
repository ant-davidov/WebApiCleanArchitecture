using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    internal class RabbitMqConfig
    {
        public string HostName { get; set; }
        public string QueueName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
