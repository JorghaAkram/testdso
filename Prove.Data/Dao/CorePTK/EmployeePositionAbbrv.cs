using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class EmployeePositionAbbrv : BaseDao
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
