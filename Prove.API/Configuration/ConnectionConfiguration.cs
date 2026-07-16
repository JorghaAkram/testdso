using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prove.BusinessLogic.Impl;
using Prove.BusinessLogic.Interface;
using Prove.Data.Data;
using Prove.Data.Services.CorePTK;
using Prove.Data.Services.CorePTK.Interface;
using Prove.Data.Services.CronJob;
using Prove.Data.Services.Prove;
using Prove.Data.Services.Prove.Interface;
using Prove.Data.Services.SendMail;
using Prove.Data.Services.SendMail.Interface;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Configuration
{
    public class ConnectionConfiguration
    {
        public static void GetService(IServiceCollection services, IConfiguration configuration, bool IsProduction)
        {
            var appSettings = configuration.Get<AppSetting>();
            var connection = IsProduction ? appSettings.ConnectionStrings[GeneralConstant.ProdConnectionMode] : appSettings.ConnectionStrings[GeneralConstant.DevConnectionMode];

            //services.AddDbContext<AppUserCoreDbContext>(options
            //    => options.UseSqlServer(connection + appSettings.DataBase[DbConstant.AppUserDb.ToString()]));

            services.AddDbContext<CorePTKContext>(options
                => options.UseSqlServer(connection + appSettings.DataBase[DbConstant.CorePTKDb.ToString()]));

            services.AddDbContext<CronJobContext>(options
                => options.UseSqlServer(connection + appSettings.DataBase[DbConstant.CronJobDb.ToString()]));

            services.AddDbContext<ProveExtContext>(options
                => options.UseSqlServer(connection + appSettings.DataBase[DbConstant.ProveExtDb.ToString()]));

            services.AddDbContext<UsmanContext>(options
                => options.UseSqlServer(connection + appSettings.ConnectionStrings[DbConstant.UsmanDb.ToString()]));
        }

        public enum DbConstant
        {
            //AppUserDb,
            CorePTKDb,
            ProveExtDb,
            CronJobDb,
            UsmanDb
        }

        public static void GetScoped(IServiceCollection services)
        {
            services.AddScoped<IGlossaryRepo, GlossaryRepoImpl>();
            services.AddScoped<IMasterDataRepo, MasterDataRepoImpl>();
            services.AddScoped<IRegulationSKRepo, RegulationSKRepoImpl>();
            services.AddScoped<IRegulationSPRepo, RegulationSPRepoImpl>();
            services.AddScoped<IRegulationSTKRepo, RegulationSTKRepoImpl>();
            services.AddScoped<ITemplateRepo, TemplateRepoImpl>();
            services.AddScoped<IReportRepo, ReportRepoImpl>();
            services.AddScoped<IProveProbis, ProveProbisService>();
            services.AddScoped<IFileUploadRepo, FileUploadRepoImpl>();

            services.AddScoped<ISendMail, CronEmailService>();
            services.AddScoped<IEmailJobs, EmailJobsService>();
            //services.AddScoped<IActivityLog, ActivityLogService>();
        }
    }
}
