using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class ShipOwner : BaseDao
    {
        [Required]
        [StringLength(50, ErrorMessage = "Limit owner name to 50 characters.")]
        public string Name { get; set; }
        public virtual Customers Customers { get; set; }
    }
}
