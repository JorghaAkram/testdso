using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prove.Data.Dao.Prove;
using Prove.Data.Services.Prove.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Data
{
    public class ProveExtContext : IdentityDbContext<IdentityUser>//DbContext
    {
        public ProveExtContext(DbContextOptions<ProveExtContext> options) : base(options)
        {
        }
        public DbSet<Glossary> Glossary { get; set; }
        public DbSet<Probis> Probis { get; set; }
        public DbSet<ProductOfLawSKSP> ProductOfLawSKSP { get; set; }
        public DbSet<ProductOfLawSTK> ProductOfLawSTK { get; set; }
        public DbSet<RegulationSKSP> RegulationSKSP { get; set; }
        public DbSet<RegulationSTK> RegulationSTK { get; set; }
        public DbSet<STKType> STKType { get; set; }
        public DbSet<SaveCode> SaveCode { get; set; }
        public DbSet<Template> Template { get; set; }
        public DbSet<TemplateType> TemplateType { get; set; }
        public DbSet<RegulationHistory> RegulationHistory { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<FileUpload> FileUpload { get; set; }
        public DbSet<ActivityLog> ActivityLog { get; set; }
        public DbQuery<ProbisHierarchy> ProbisHierarchy { get; set; }
    }
}
