using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class ShipStatus : BaseDao
    {
        [Required]
        [StringLength(30, ErrorMessage = "Limit status name to 30 characters.")]
        public string Name { get; set; }
    }
}
