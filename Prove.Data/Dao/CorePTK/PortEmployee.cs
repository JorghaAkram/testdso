using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class PortEmployee : BaseDao
    {
        public virtual Employee Employee { get; set; }

        [ForeignKey("EmployeePositionId")]
        [Column("EmployeePositionId")]
        public virtual EmployeePosition EmployeePosition { get; set; }
    }
}
