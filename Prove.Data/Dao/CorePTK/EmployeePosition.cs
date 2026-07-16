using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class EmployeePosition : BaseDao
    {
        [Required]
        public virtual Organization Organization { get; set; }

        [Required]
        [StringLength(12)]
        public virtual EmployeePositionAbbrv EmployeePositionAbbrv { get; set; } // BOD, AST. Man

        [StringLength(20)]
        public string PositionIdSap { get; set; }

        [StringLength(100)]
        public string PositionName { get; set; }

        public int PrlLower { get; set; }

        public int PrlHigher { get; set; }

        [Required]
        public virtual PAreaSub PAreaSub { get; set; }

        [StringLength(30)]
        public string ExtPhoneNo { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = PrideConstant.FILLED;
    }
}
