using Prove.Data.Dao.CorePTK;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Prove.Data.Services.CorePTK.Interface
{
    public interface IActivityLog : IBaseCrud<ActivityLog>
    {
        void SaveActivityLog(string methodName, string controllerName, int id, string status, string userName, string errorMessage = null, string remark = null);

        string GetCurrentMethod([CallerMemberName] string methodName = "");
    }
}
