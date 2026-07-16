using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class KBO : BaseDao
    {
        public string Code { get; set; }
        public virtual EmployeePosition EmployeePosition { get; set; }
    }
}
