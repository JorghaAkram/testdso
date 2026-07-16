using Microsoft.EntityFrameworkCore;
using Prove.Data.Dao.CronJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Data
{
    public class CronJobContext : DbContext
    {
        public CronJobContext(DbContextOptions<CronJobContext> options) : base(options) { }

        public DbSet<EmailJob> EmailJobs { get; set; }
    }
}
