using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Usman
{
    public class Role : BaseDao
    {
        public string RoleName { get; set; }
        public virtual Application Application { get; set; }
    }
}
