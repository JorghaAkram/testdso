using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Prove
{
    public class ProductOfLawSTK : BaseDao
    {
        public virtual STKType Type { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public string CompanyCode { get; set; }
    }
}
