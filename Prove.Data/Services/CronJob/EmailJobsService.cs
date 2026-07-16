using Prove.Data.Dao.CronJob;
using Prove.Data.Data;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.CronJob
{
    public class EmailJobsService : BaseService<EmailJob>, IEmailJobs
    {
        public EmailJobsService(CronJobContext context) : base(context)
        {

        }
    }
}
