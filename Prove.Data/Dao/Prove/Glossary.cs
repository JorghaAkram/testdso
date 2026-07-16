using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Prove
{
    public class Glossary : BaseDao
    {
        [StringLength(100)]
        public string Term { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Reference { get; set; }

        public string CompanyCode { get; set; }
    }
}
