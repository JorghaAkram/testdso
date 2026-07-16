using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class MaritalStatus : BaseDao
    {
        [Required]
        [StringLength(50, ErrorMessage = PrideConstant.ErrorMessageFieldLength)]
        public string Name { get; set; }
    }
}
