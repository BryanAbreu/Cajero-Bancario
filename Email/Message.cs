using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Email
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }

        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to,string subject,string content)
        {
            this.Subject = subject;
            this.Content = content;

            this.To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x))); 
        }
    }
}
