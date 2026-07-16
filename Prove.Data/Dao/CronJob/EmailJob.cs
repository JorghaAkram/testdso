using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CronJob
{
    public class EmailJob
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public string Application { get; set; }
        public string Module { get; set; }
        public string Subject { get; set; }
        [DataType(DataType.Text)]
        public string Body { get; set; }
        public string From { get; set; }
        [DataType(DataType.Text)]
        public string EmailAddresses { get; set; }
        [DataType(DataType.Text)]
        public string AttachmentPath { get; set; }
        public string Status { get; set; } = EmailSerivceStatus.IN_QUEUE.ToString();
        public int NumberOfTrials { get; set; }
    }

    public enum EmailSerivceStatus
    {
        IN_QUEUE,
        IN_PROCESS,
        FAILED,
        DONE,
        IGNORED
    }
}
