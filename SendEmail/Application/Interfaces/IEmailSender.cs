using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendEmail.Domain;

namespace SendEmail.Application.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(Message email);
    }
}
