using Prove.Data.Dao.CorePTK;
using Prove.Data.Data;
using Prove.Data.Services.CorePTK.Interface;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Prove.Data.Services.CorePTK
{
    public class ActivityLogService : BaseServicePride<ActivityLog>, IActivityLog
    {
        public ActivityLogService(CorePTKContext context) : base(context)
        {
        }

        string IActivityLog.GetCurrentMethod([CallerMemberName] string methodName = "")
        {
            return methodName.ToUpper(); //output will be name of calling method
        }

        void IActivityLog.SaveActivityLog(string methodName, string controllerName, int id, string status, string userName, string errorMessage, string remark)
        {
            ActivityLog activityLog = new ActivityLog
            {
                Controller = controllerName
                .ToString()
                .ToUpper(),
                Method = methodName,
                TrxId = id,
                UserName = userName,
                Status = status,
                ErrorMessage = errorMessage,
                Remark = remark
            };
            Add(activityLog);
        }
    }
}
