using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Prove
{
    public class ProductOfLawSKSP : BaseDao
    {
        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(10)]
        public string KBO { get; set; }

        [StringLength(10)]
        public string PositionNumber { get; set; }

        public string CompanyCode { get; set; }
    }
}
