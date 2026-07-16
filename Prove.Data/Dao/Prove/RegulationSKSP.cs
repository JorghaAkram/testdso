using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Prove
{
    public class RegulationSKSP : BaseDao
    {
        public RegulationCategory RegCategoryId { get; set; }
        public virtual ProductOfLawSKSP ProductOfLawSKSP { get; set; }

        [StringLength(5)]
        public string Code { get; set; }

        [StringLength(5)]
        public string Number { get; set; }

        [StringLength(10)]
        public string PositionNumber { get; set; }

        [StringLength(10)]
        public string KBO { get; set; }

        public virtual SaveCode SaveCode { get; set; }

        [StringLength(50)]
        public string SKSPNumber { get; set; }

        public string Title { get; set; }
        public DateTime TmtApplies { get; set; }
        public StatusDoc StatusDocId { get; set; }
        public string Status { get; set; }
        public TypeSurat TypeId { get; set; }
        public int FileId { get; set; }
        public int FileSupportId { get; set; }

        [StringLength(5)]
        public string Year { get; set; }

        public string ConceptorId { get; set; }
        public string ConceptorDirId { get; set; }
        public string ConceptorDivId { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public DateTime? JointReviewDate { get; set; }
        public string CompanyCode { get; set; }
    }
}
