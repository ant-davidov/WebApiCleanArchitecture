using SendEmail.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.Application.Interfaces
{
    public interface IMessageQueueSubscriber
    {
        void Subscribe();
    }
}
