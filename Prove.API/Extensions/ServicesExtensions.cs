using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Minio.AspNetCore;
using Prove.API.Configuration;
using Prove.BusinessLogic.Impl;
using Prove.BusinessLogic.Interface;
using Prove.Data.Configuration;
using Prove.Data.Dao.CorePTK;
using Prove.Data.Data;
using Prove.Data.Services.CorePTK;
using Prove.Data.Services.CorePTK.Interface;
using Prove.Data.Services.CronJob;
using Prove.Data.Services.FileUpload;
using Prove.Data.Services.FileUpload.Interface;
using Prove.Data.Services.Prove;
using Prove.Data.Services.Prove.Interface;
using Prove.Data.Services.SendMail;
using Prove.Data.Services.SendMail.Interface;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Prove.API.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var AppSetting = configuration.Get<AppSetting>();
            GeneralConstant.IsProduction = AppSetting.IsProduction;
            var connection = AppSetting.IsProduction ? AppSetting.ConnectionStrings["ProdConnectionMode"] : AppSetting.ConnectionStrings["DevConnectionMode"];

            services.AddDbContext<ProveExtContext>(options
                => options.UseSqlServer(connection + AppSetting.DataBase[DatabaseEnums.ProveExtDb.ToString()]));
            services.AddDbContext<CronJobContext>(options
                => options.UseSqlServer(connection + AppSetting.DataBase[DatabaseEnums.CronJobDb.ToString()]));
            services.AddDbContext<CorePTKContext>(options
                => options.UseSqlServer(connection + AppSetting.DataBase[DatabaseEnums.CorePTKDb.ToString()]));
            services.AddDbContext<UsmanContext>(options
                => options.UseSqlServer(connection + AppSetting.DataBase[DatabaseEnums.UsmanDb.ToString()]));
        }

        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ProveExtContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.Get<AppSetting>().Secret["Issuer"],
                        ValidAudience = configuration.Get<AppSetting>().Secret["Audience"],

                        // Set signing key
                        IssuerSigningKey = new SymmetricSecurityKey(
                            // Get our secret key from configuration
                            Encoding.ASCII.GetBytes(configuration.Get<AppSetting>().Secret["SecretKey"])),
                    };
                });
        }

        public static void ConfigureMinio(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.Get<AppSettings>();
            string a = configuration.GetSection("MinioService").GetSection("AccessKey").Value;
            string b = appSettings.IsProduction ? configuration.GetSection("MinioService").GetSection("URL").GetSection("Prod").Value : configuration.GetSection("MinioService").GetSection("URL").GetSection("Dev").Value;
            services.AddMinio(opt =>
            {
                opt.Endpoint = appSettings.IsProduction ? configuration.GetSection("MinioService").GetSection("URL").GetSection("Prod").Value : configuration.GetSection("MinioService").GetSection("URL").GetSection("Dev").Value;
                opt.AccessKey = configuration.GetSection("MinioService").GetSection("AccessKey").Value;
                opt.SecretKey = configuration.GetSection("MinioService").GetSection("SecretKey").Value;
            });
        }

        public static void ConfigureHTTPClientFactory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("IDAMAN.API", async o =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var jwt = serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.Request?.Headers["Authorization"];

                //o.BaseAddress = new Uri("https://rest.idaman.pertamina.com");
                o.BaseAddress = new Uri(configuration.Get<AppSetting>().BaseURL["Idaman"]);

                o.DefaultRequestHeaders.Add("Authorization", $"{jwt.Value}");
            });
        }

        public static void InjectDataLayer(this IServiceCollection services)
        {
        }
        public static void InjectDomainLayer(this IServiceCollection services)
        {
            services.AddScoped<IGlossaryRepo, GlossaryRepoImpl>();
            services.AddScoped<IMasterDataRepo, MasterDataRepoImpl>();
            services.AddScoped<IRegulationSKRepo, RegulationSKRepoImpl>();
            services.AddScoped<IRegulationSPRepo, RegulationSPRepoImpl>();
            services.AddScoped<IRegulationSTKRepo, RegulationSTKRepoImpl>();
            services.AddScoped<IReportRepo, ReportRepoImpl>();
            services.AddScoped<ITemplateRepo, TemplateRepoImpl>();

            services.AddScoped<IFileUploadRepo, FileUploadRepoImpl>();
            services.AddScoped<ISendMail, CronEmailService>();
            services.AddScoped<IEmailJobs, EmailJobsService>();
            services.AddScoped<IProveProbis, ProveProbisService>();
            services.AddScoped<IActivityLog, ActivityLogService>();
        }
    }
}
