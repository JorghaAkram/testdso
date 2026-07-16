using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class ShipArea : BaseDao
    {
        public virtual Ship Ship { get; set; }
        public virtual PArea PArea { get; set; } = null!;
        public virtual PAreaSub PAreaSub { get; set; } = null!;
    }
}
