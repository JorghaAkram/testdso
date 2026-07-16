using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class Bank : BaseDao
    {
        [StringLength(10, ErrorMessage = PrideConstant.ErrorMessageFieldLength)]
        public string Code { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = PrideConstant.ErrorMessageFieldLength)]
        public string Name { get; set; }

        [StringLength(20, ErrorMessage = PrideConstant.ErrorMessageFieldLength)]
        public string Alias { get; set; }
    }
}
