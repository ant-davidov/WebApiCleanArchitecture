using SendEmail.Application.Interfaces;
using SendEmail.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.Infrastructure
{
    public class EmailSender : IEmailSender
    {
        public Task<bool> SendEmail(Message email)
        {
            Console.WriteLine($"mail - {email.Body}");
            return Task.FromResult(true);
        }
    }
}
