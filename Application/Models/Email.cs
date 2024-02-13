using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Email
    {
        public string EmailFrom {get; set;}
        public string NameFrom { get; set;}
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
