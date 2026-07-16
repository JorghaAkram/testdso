using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.BusinessLogic.Models
{
    public class ReportSKSPJtable : BaseJtable
    {
        public string produkHukumName { get; set; }
        public int produkHukumId { get; set; }
        public int regulationCategoryId { get; set; }
        public string regulationCategoryName { get; set; }
        public string nomorSKSP { get; set; }
        public string perihal { get; set; }
        public DateTime tmtBerlaku { get; set; }
        public DateTime? expiredDate { get; set; }
        public DateTime? jointReviewDate { get; set; }
        public string code { get; set; }
        public string nomor { get; set; }
        public string positionNumber { get; set; }
        public string kbo { get; set; }
        public string kodeSimpan { get; set; }
        public int kodeSimpanId { get; set; }
        public string statusDoc { get; set; }
        public int statusDocId { get; set; }
        public string status { get; set; }
        public int fileId { get; set; }
        public int fileSupportId { get; set; }
        public string fileName { get; set; }
        public string fileSupportName { get; set; }
        public string type { get; set; }
        public string tahun { get; set; }
    }

    public class ReportSKSPJtableParam : BaseDTParam
    {
        public int produkHukum { get; set; }
        public string nomor { get; set; }
        public string perihal { get; set; }
        public DateTime? tmtBerlaku { get; set; }
        public string status { get; set; }
        public string documentType { get; set; }
        public string companyCode { get; set; }
    }

    public class ReportSTKJtable : BaseJtable
    {
        public string produkHukumName { get; set; }
        public int produkHukumId { get; set; }
        public int regulationCategoryId { get; set; }
        public string regulationCategoryName { get; set; }
        public string jenis { get; set; }
        public string nomor { get; set; }
        public string nomorSerial { get; set; }
        public string nomorSTK { get; set; }
        public string positionNumber { get; set; }
        public string positionName { get; set; }
        public string kbo { get; set; }
        public string year { get; set; }
        public string kodeSimpan { get; set; }
        public int kodeSimpanId { get; set; }
        public int revisedFlag { get; set; }
        public string statusDoc { get; set; }
        public int statusDocId { get; set; }
        public string perihal { get; set; }
        public string positionCode { get; set; }
        public DateTime tmtBerlaku { get; set; }
        public string probis { get; set; }
        public int probisId { get; set; }
        public string probisNumber { get; set; }
        public string status { get; set; }
        public int fileId { get; set; }
        public int fileSupportId { get; set; }
        public string fileName { get; set; }
        public string fileSupportName { get; set; }
    }

    public class ReportSTKJtableParam : BaseDTParam
    {
        public int jenis { get; set; }
        public int revisiKe { get; set; }
        public string fungsi { get; set; }
        public string nomor { get; set; }
        public string perihal { get; set; }
        public DateTime? tmtBerlaku { get; set; }
        public string status { get; set; }
        public string documentType { get; set; }
        public string companyCode { get; set; }
    }
}
