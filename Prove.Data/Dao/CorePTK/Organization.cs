using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class Organization : BaseDao
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } // PTK etc.

        [Required]
        [StringLength(50)]
        public string SKNo { get; set; }

        public int Year { get; set; }

        public string Description { get; set; } // keterangan atau remark
    }
}
