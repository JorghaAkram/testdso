using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Usman
{
    public class UserRole : BaseDao
    {
        public virtual Role Role { get; set; }
        public virtual PositionUser PositionUser { get; set; }
    }
}
