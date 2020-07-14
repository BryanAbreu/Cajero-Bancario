using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Email
{
    public interface IEmailSender
    {
        Task SendMailAsync(Message message);
    } 
}
