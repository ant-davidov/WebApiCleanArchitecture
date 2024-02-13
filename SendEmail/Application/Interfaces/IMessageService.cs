using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SendEmail.Domain;

namespace SendEmail.Application.Interfaces
{
    public interface IMessageService
    {
        void ProcessMessage(Message message);
    }
}
