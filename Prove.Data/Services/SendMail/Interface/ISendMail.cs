using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Prove.Data.Services.SendMail.Interface
{
    public interface ISendMail
    {
        Task Send(string subject, string body, IEnumerable<MailAddress> mailAddresses, string from = "noreply.ptk@pertamina.com", string applicationName = "", string moduleName = "", bool isSkipProcess = false);

        Task Send(string subject, string body, MailAddress mailAddress, string from = "noreply.ptk@pertamina.com", string applicationName = "", string moduleName = "", bool isSkipProcess = false);

        Task SendWithFileAttachment(string subject, string body, MailAddress mailAddress, string fileAttachmentPath, string from = "noreply.ptk@pertamina.com", string applicationName = "", string moduleName = "", bool isSkipProcess = false);
    }
}
