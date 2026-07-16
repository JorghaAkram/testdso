using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Prove
{
    public class RegulationHistory : BaseDao
    {
        public string Number { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public virtual RegulationSKSP RegulationSKSP { get; set; }
        public virtual RegulationSTK RegulationSTK { get; set; }
        public string Notes { get; set; }
        public int FileId { get; set; }
        public int FileSupportId { get; set; }
    }
}
