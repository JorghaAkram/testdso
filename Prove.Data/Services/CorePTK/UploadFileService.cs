using Prove.Data.Dao.CorePTK;
using Prove.Data.Data;
using Prove.Data.Services.CorePTK.Interface;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.CorePTK
{
    public class UploadFileService : BaseServicePride<UploadFile>, IUploadFile
    {
        public UploadFileService(CorePTKContext context) : base(context)
        {
        }
    }
}
