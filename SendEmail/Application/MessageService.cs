using SendEmail.Application.Interfaces;
using SendEmail.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.Application
{
    internal class MessageService : IMessageService
    {
        private readonly IEmailSender _emailSender;
        public MessageService(IEmailSender emailSender)
        {
           
            _emailSender = emailSender;
        }
        public async void ProcessMessage(Message message)
        {
           
           await  _emailSender.SendEmail(message);
        }
    }
}
