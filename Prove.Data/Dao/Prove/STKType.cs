using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Prove
{
    public class STKType
    {
        public int Id { get; set; }

        [StringLength(1)]
        public string Code { get; set; }
    }
}
