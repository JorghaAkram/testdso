using Microsoft.Extensions.Configuration;
using Prove.Data.Services.CorePTK.Interface;
using Prove.Data.Services.SendMail.Interface;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Prove.Data.Services.SendMail
{
    public class SendMailService : ISendMail
    {
        private readonly IConfiguration _config;
        private readonly IActivityLog _activityLog;
        private readonly IConfigurationSection section;

        public SendMailService(IConfiguration config, IActivityLog activityLog)
        {
            _config = config;
            section = _config.GetSection("EmailCredentials");
            _activityLog = activityLog;
        }

        /// <summary>
        /// Send batch email to many address
        /// </summary>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="mailAddresses"></param>
        /// <returns></returns>
        public async Task Send(string subject, string body, IEnumerable<MailAddress> mailAddresses, string from = "noreply.ptk@pertamina.com", string applicationName = "", string moduleName = "", bool isSkipProcess = false)
        {
            MailMessage objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient
            {
                Port = 25,
                Host = "mail.pertamina.com",
                Timeout = 20000,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(section.GetValue<string>("UserName"), section.GetValue<string>("Password"))
            };
            objeto_mail.From = new MailAddress(from);

            //add each email adress
            foreach (MailAddress m in mailAddresses)
            {
                objeto_mail.To.Add(m);
            }

            objeto_mail.Subject = subject;

            objeto_mail.Body = body;
            objeto_mail.IsBodyHtml = true;
            try
            {
                client.SendCompleted += (s, e) =>
                {
                    client.Dispose();
                    objeto_mail.Dispose();
                };
                await client.SendMailAsync(objeto_mail);
                //await Task.Run(() =>
                //{
                //    client.Send(objeto_mail);
                //});
            }
            catch (Exception e)
            {
                _activityLog.SaveActivityLog(nameof(Send), nameof(SendMailService), 0, GeneralConstant.FAILED, (subject.Length > 100 ? subject.Substring(0, 100) : subject), e.InnerException?.Message ?? e.Message, "SEND EMAIL FAILED");
                throw new SendMailFailedException(e.InnerException?.Message ?? e.Message);
            }
        }

        /// <summary>
        /// send emil to spesific address
        /// </summary>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        public async Task Send(string subject, string body, MailAddress mailAddress, string from = "noreply.ptk@pertamina.com", string applicationName = "", string moduleName = "", bool isSkipProcess = false)
        {
            MailMessage objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient
            {
                Port = 25,
                Host = "mail.pertamina.com",
                Timeout = 20000,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(section.GetValue<string>("UserName"), section.GetValue<string>("Password"))
            };
            objeto_mail.From = new MailAddress(from);

            objeto_mail.To.Add(mailAddress);

            objeto_mail.Subject = subject;

            objeto_mail.Body = body;
            objeto_mail.IsBodyHtml = true;
            try
            {
                client.SendCompleted += (s, e) =>
                {
                    client.Dispose();
                    objeto_mail.Dispose();
                };
                await client.SendMailAsync(objeto_mail);
                //await Task.Run(() =>
                //{
                //    client.Send(objeto_mail);
                //});
            }
            catch (Exception e)
            {
                _activityLog.SaveActivityLog(nameof(Send), nameof(SendMailService), 0, GeneralConstant.FAILED, (subject.Length > 100 ? subject.Substring(0, 100) : subject), e.InnerException?.Message ?? e.Message, "SEND EMAIL FAILED");
                throw new SendMailFailedException(e.InnerException?.Message ?? e.Message);
            }
        }

        /// <summary>
        /// send emil to spesific address
        /// </summary>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        public async Task SendWithFileAttachment(string subject, string body, MailAddress mailAddress, string fileAttachmentPath, string from = "noreply.ptk@pertamina.com", string applicationName = "", string moduleName = "", bool isSkipProcess = false)
        {
            MailMessage objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient
            {
                Port = 25,
                Host = "mail.pertamina.com",
                Timeout = 20000,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(section.GetValue<string>("UserName"), section.GetValue<string>("Password"))
            };
            objeto_mail.From = new MailAddress(from);

            objeto_mail.To.Add(mailAddress);

            objeto_mail.Subject = subject;

            objeto_mail.Body = body;
            objeto_mail.IsBodyHtml = true;

            objeto_mail.Attachments.Add(new Attachment(fileAttachmentPath));
            try
            {
                client.SendCompleted += (s, e) =>
                {
                    client.Dispose();
                    objeto_mail.Dispose();
                };
                await client.SendMailAsync(objeto_mail);
                //await Task.Run(() =>
                //{
                //    client.Send(objeto_mail);
                //});
            }
            catch (Exception e)
            {
                _activityLog.SaveActivityLog(nameof(SendWithFileAttachment), nameof(SendMailService), 0, GeneralConstant.FAILED, (subject.Length > 100 ? subject.Substring(0, 100) : subject), e.InnerException?.Message ?? e.Message, "SEND EMAIL FAILED");
                throw new SendMailFailedException(e.InnerException?.Message ?? e.Message);
            }
        }
    }
}
