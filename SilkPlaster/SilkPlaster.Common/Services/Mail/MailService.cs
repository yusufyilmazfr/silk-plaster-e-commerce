using SilkPlaster.Common.Utilities.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.Services.Mail
{
    public class MailService : IMailService
    {
        public void SendMailAsync(string body, string to, string subject, bool isHtml = true)
        {
            SendMailAsync(body, new List<string> { to }, subject, isHtml);
        }

        public void SendMailAsync(string body, List<string> to, string subject, bool isHtml = true)
        {
            Task.Run(() =>
            {
                try
                {
                    var message = new MailMessage();
                    message.From = new MailAddress(ConfigHelper.Get<string>("MailUser"));

                    to.ForEach(
                        x => message.To.Add(new MailAddress(x)
                    ));

                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = isHtml;

                    using (var smtp = new SmtpClient(ConfigHelper.Get<string>("MailHost"), ConfigHelper.Get<int>("MailPort")))
                    {
                        smtp.EnableSsl = true;

                        smtp.Credentials = new NetworkCredential(
                                ConfigHelper.Get<string>("MailUser"),
                                ConfigHelper.Get<string>("MailPassword")
                        );

                        smtp.Send(message);
                    }

                }
                catch
                {
                }

            });
        }
    }
}
