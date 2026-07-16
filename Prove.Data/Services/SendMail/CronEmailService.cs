using Microsoft.Extensions.Configuration;
using Prove.Data.Dao.CronJob;
using Prove.Data.Services.CronJob;
using Prove.Data.Services.SendMail.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Prove.Data.Services.SendMail
{
    public class CronEmailService : ISendMail
    {
        private readonly IEmailJobs _emailJob;
        private readonly IConfiguration _configuration;

        public CronEmailService(IEmailJobs emailJob, IConfiguration configuration)
        {
            _emailJob = emailJob;
            _configuration = configuration;
        }

        private async Task Notify()
        {
            bool isProduction = _configuration.GetValue<bool>("IsProduction");
            string baseServicePath = isProduction ? _configuration.GetSection("EmailService").GetValue<string>("Prod") : _configuration.GetSection("EmailService").GetValue<string>("Dev");

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(baseServicePath)
            };
            //client.BaseAddress = new Uri(baseServicePath);
            await client.GetAsync("Email/Process");
        }

        public async Task Send(string subject, string body, IEnumerable<MailAddress> mailAddresses, string from = "noreply.ptk@pertamina.com", string applicationName = "", string moduleName = "", bool isSkipProcess = false)
        {
            EmailJob emailJob = new EmailJob
            {
                Application = "",
                Module = "",
                Body = body,
                From = from,
                Subject = subject,
                EmailAddresses = string.Join(",", mailAddresses.Select(b => b.Address))
            };
            await _emailJob.AddAsync(emailJob);
            if (!isSkipProcess)
                await Notify();
        }

        public async Task Send(string subject, string body, MailAddress mailAddress, string from = "noreply.ptk@pertamina.com", string applicationName = "", string moduleName = "", bool isSkipProcess = false)
        {
            EmailJob emailJob = new EmailJob
            {
                Application = "",
                Module = "",
                Body = body,
                From = from,
                Subject = subject,
                EmailAddresses = mailAddress.Address
            };
            await _emailJob.AddAsync(emailJob);
            if (!isSkipProcess)
                await Notify();
        }

        public async Task SendWithFileAttachment(string subject, string body, MailAddress mailAddress, string fileAttachmentPath, string from = "noreply.ptk@pertamina.com", string applicationName = "", string moduleName = "", bool isSkipProcess = false)
        {
            EmailJob emailJob = new EmailJob
            {
                Application = "",
                Module = "",
                Body = body,
                Subject = subject,
                EmailAddresses = mailAddress.Address,
                From = from,
                AttachmentPath = fileAttachmentPath
            };
            await _emailJob.AddAsync(emailJob);
            if (!isSkipProcess)
                await Notify();
        }
    }
}
