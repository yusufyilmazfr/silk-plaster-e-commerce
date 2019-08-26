using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.Services.Mail
{
    public interface IMailService
    {
        void SendMailAsync(string body, string to, string subject, bool isHtml = true);
        void SendMailAsync(string body, List<string> to, string subject, bool isHtml = true);
    }
}
