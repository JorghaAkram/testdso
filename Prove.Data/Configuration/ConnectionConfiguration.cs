using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prove.Data.Data;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Configuration
{
    public static class ConnectionConfiguration
    {
        public static void GetService(IServiceCollection services, IConfiguration configuration, bool IsProduction)
        {
            var appSettings = configuration.Get<AppSettings>();
            var connection = IsProduction ? appSettings.Default[GeneralConstant.ProdConnectionMode] : appSettings.Default[GeneralConstant.DevConnectionMode];

            services.AddDbContext<CorePTKContext>(options
                => options.UseSqlServer(connection + appSettings.ConnectionStrings[DbConstant.CorePTKDb.ToString()]));

            services.AddDbContext<CronJobContext>(options
                => options.UseSqlServer(connection + appSettings.ConnectionStrings[DbConstant.CronJobDb.ToString()]));

            services.AddDbContext<ProveExtContext>(options
                => options.UseSqlServer(connection + appSettings.ConnectionStrings[DbConstant.ProveExtDb.ToString()]));

            services.AddDbContext<UsmanContext>(options
                => options.UseSqlServer(connection + appSettings.ConnectionStrings[DbConstant.UsmanDb.ToString()]));
        }

        public enum DbConstant
        {
            CorePTKDb,
            ProveExtDb,
            CronJobDb,
            UsmanDb
        }
    }
}
