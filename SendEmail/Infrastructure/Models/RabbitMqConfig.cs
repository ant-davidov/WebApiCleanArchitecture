using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.Infrastructure.Models
{
    public class RabbitMqConfig
    {
        public string Uri { get; set; }
        public string QueueName { get; set; }
    }
}
