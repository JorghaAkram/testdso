using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Prove
{
    public class RegulationSTK : BaseDao
    {
        public RegulationCategory RegCategoryId { get; set; }
        public virtual ProductOfLawSTK ProductOfLawSTK { get; set; }
        public virtual STKType Type { get; set; }

        [StringLength(8)]
        public string Number { get; set; }

        [StringLength(8)]
        public string SerialNumberSTK { get; set; }

        [StringLength(50)]
        public string RefNumber { get; set; }

        [StringLength(10)]
        public string PositionId { get; set; }

        [StringLength(10)]
        public string KBO { get; set; }

        [StringLength(5)]
        public string Year { get; set; }

        public virtual SaveCode SaveCode { get; set; }

        [StringLength(100)]
        public string STKNumber { get; set; }

        public int RevisedFlag { get; set; }
        public StatusDoc StatusDocId { get; set; }
        public string Title { get; set; }

        [StringLength(10)]
        public string PositionCode { get; set; }

        public DateTime TmtApplies { get; set; }
        public Probis Probis { get; set; }

        [StringLength(2)]
        public string ProbisNumber { get; set; }

        public string Status { get; set; }
        public int FileId { get; set; }
        public int FileSupportId { get; set; }
        public string ConceptorId { get; set; }
        public string ConceptorDirId { get; set; }
        public string ConceptorDivId { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public DateTime? JointReviewDate { get; set; }
        public string CompanyCode { get; set; }
    }
}
