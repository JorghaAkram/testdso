using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class EmployeePositionStructs : BaseDao
    {
        [Required]
        public virtual EmployeePosition EmployeePosition { get; set; }

        public virtual EmployeePosition Leader { get; set; }
    }
}
