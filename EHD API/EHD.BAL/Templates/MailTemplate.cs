using EHD.BAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EHD.BAL.Domain_Models;

namespace EHD.BAL.Templates
{
    public class MailTemplate : IMailTemplate
    {
        public async Task SendMail(MailTemplateDTO data)
        {
            string fromAddress = "thanushree.u@joyitsolutions.co";
            string password = "Joyit@321";
            string mailHeader = $"<html><body><h1>{data.MailHeader}<h1></body></html>";
            string mailBody = $"<html><body><h3>{data.MailBody}<h3/></body></html>";
            string mailFooter = $"<html><body><h1>{data.MailFooter}</h1></body></html>";
            string mailContent = mailHeader + mailBody + mailFooter;

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(fromAddress);
                message.To.Add(new MailAddress(data.ToAddress));
                message.Subject = data.Subject;
                message.Body = mailContent;
                message.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient("smtp.office365.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential(fromAddress, password);
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(message);
                }
            }
        }
    }
}
