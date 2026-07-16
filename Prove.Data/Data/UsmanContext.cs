using Microsoft.EntityFrameworkCore;
using Prove.Data.Dao.Usman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Data
{
    public class UsmanContext : DbContext
    {
        public UsmanContext(DbContextOptions<UsmanContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Application> Application { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<PositionUser> PositionUsers { get; set; }
    }
}
