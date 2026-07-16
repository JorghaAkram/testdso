using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prove.Data.Configuration;
using Prove.Data.Dao.Prove;
using Prove.Data.Data;
using Prove.Data.Services.Prove.Interface;
using Prove.Data.Services.Prove.Models;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Prove.Data.Configuration.ConnectionConfiguration;

namespace Prove.Data.Services.Prove
{
    public class ProveProbisService : BaseService<Probis>, IProveProbis
    {
        private IConfiguration _configuration;

        public ProveProbisService(ProveExtContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public IDbConnection Connection
        {
            get
            {
                var appSettings = _configuration.Get<AppSettings>();
                return new SqlConnection(appSettings.ConnectionStrings[GeneralConstant.ISPRODUCTION ? "ProdConnectionMode" : "DevConnectionMode"] + appSettings.DataBase[DbConstant.ProveExtDb.ToString()]);
            }
        }

        //private string DataBase = "STKDb";

        public async Task<List<ProbisHierarchy>> GetProbisHierarchy()
        {
            string query = $@"
            EXEC SP_GET_HIERARCHY_PROBIS
            ";

            List<ProbisHierarchy> result = await _context.Query<ProbisHierarchy>().FromSql(query).ToListAsync(); // _context.Query<ProbisHierarchy>().FromSql(query).ToListAsync();

            return result;
        }
    }
}
