using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Prove
{
    public class Probis : BaseDao
    {
        [StringLength(50)]
        public string ParentProbisNumber { get; set; }

        public int ChildProbisNumber { get; set; }
        public string Description { get; set; }
        public int Under_Parent { get; set; }
    }
}
